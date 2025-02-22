using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SourceGenerator.Test.Utils
{
    public static class MsTestAssertThatExtensions
    {

        /// <summary>Extension method for use with <see cref="Assert.That"/> to validate two <see cref="GeneratorDriverRunResult"/> are equivalent</summary>
        /// <param name="_">Unused, provides <see cref="Assert.That"/> extension support</param>
        /// <param name="r1">Results of first run</param>
        /// <param name="r2">Results of second run</param>
        /// <param name="trackingNames">Names of custom tracking steps to validate</param>
        public static void AreEqual(
            this Assert _,
            GeneratorDriverRunResult r1,
            GeneratorDriverRunResult r2,
            ImmutableArray<string> trackingNames
            )
        {
            var trackedSteps1 = r1.GetTrackedSteps(trackingNames);
            var trackedSteps2 = r2.GetTrackedSteps(trackingNames);

            // Assert the static requirements
            Assert.AreNotEqual(0, trackedSteps1.Count, "Should not be an empty set of steps matching tracked names");
            Assert.AreEqual(trackedSteps1.Count, trackedSteps2.Count, "Both runs should have same number of tracked steps");
            bool hasSameKeys = trackedSteps1.Zip(trackedSteps2, (s1, s2) => trackedSteps2.ContainsKey(s1.Key) && trackedSteps1.ContainsKey(s2.Key))
                                            .All(x => x);
            Assert.IsTrue(hasSameKeys, "Both sets of runs should have the same keys");

            // loop through all KVP of name to step in result set 1
            // assert that the second run steps for the same tracking name are equal.
            foreach (var stepInfo in trackedSteps1)
            {
                var runSteps2 = trackedSteps2[stepInfo.Key];
                Assert.That.AreEqual(stepInfo.Value, runSteps2, stepInfo.Key);
            }
        }

        /// <summary>
        /// Extension method for use with <see cref="Assert.That"/> to validate each member of a pair of <see cref="ImmutableArray{IncrementalGeneratorRunStep}"/>
        /// are equivalent.
        /// </summary>
        /// <param name="_">Unused, provides <see cref="Assert.That"/> extension support</param>
        /// <param name="steps1">Array of steps to test against</param>
        /// <param name="steps2">Array of steps to assert are equal to the elements of <paramref name="steps1"/></param>
        /// <param name="stepTrackingName">Tracking name of the step for use in diagnostic messages</param>
        /// <remarks>
        /// This uses the built-in extensibility point <see cref="Assert.That"/> to perform asserts on
        /// each member of the input arrays. Each is tested for equality and this only passes
        /// if ALL members are equal.
        /// </remarks>
        public static void AreEqual(
            this Assert _,
            ImmutableArray<IncrementalGeneratorRunStep> steps1,
            ImmutableArray<IncrementalGeneratorRunStep> steps2,
            string stepTrackingName
            )
        {
            Assert.AreEqual(steps1.Length, steps2.Length, "Step lengths should be equal");
            for (int i = 0; i < steps1.Length; ++i)
            {
                var runStep1 = steps1[i];
                var runStep2 = steps2[i];

                IEnumerable<object> outputs1 = runStep1.Outputs.Select(x => x.Value);
                IEnumerable<object> outputs2 = runStep2.Outputs.Select(x => x.Value);

                Assert.AreEqual(outputs1, outputs2, EnumerableObjectComparer.Default, $"{stepTrackingName} should produce cacheable outputs");
                Assert.That.OutputsCachedOrUnchanged(runStep2, stepTrackingName);
                Assert.That.ObjectGraphContainsValidSymbols(runStep1, stepTrackingName);
            }
        }

        /// <summary>Extension method for use with <see cref="Assert.That"/> to assert all of the tracked output steps are cached</summary>
        /// <param name="_">Unused, provides <see cref="Assert.That"/> extension support</param>
        /// <param name="driverRunResult">Run results to test for cached outputs</param>
        public static void Cached(this Assert _, GeneratorDriverRunResult driverRunResult)
        {
            if (driverRunResult is null)
            {
                throw new ArgumentNullException(nameof(driverRunResult));
            }

            // verify the second run only generated cached source outputs
            var uncachedSteps = from generatorRunResult in driverRunResult.Results
                                from trackedStepKvp in generatorRunResult.TrackedOutputSteps
                                from runStep in trackedStepKvp.Value     // name is used in select if condition passes
                                from valueReasonTuple in runStep.Outputs // all outputs must have a cached reason.
                                where valueReasonTuple.Reason != IncrementalStepRunReason.Cached
                                select runStep.Name;
            foreach (string stepTrackingName in uncachedSteps)
            {
                Assert.Fail("Step name {0} contains uncached results for second run!", stepTrackingName ?? "<null>");
            }
        }

        /// <summary>Extension method for use with <see cref="Assert.That"/> to validate that an object is not of a banned type</summary>
        /// <param name="_"></param>
        /// <param name="node">object node to test</param>
        /// <param name="message">reason message for any failures</param>
        public static void NotBannedType(
            this Assert _,
            object? node,
            /*[StringSyntax(StringSyntaxAttribute.CompositeFormat)]*/ string message,
            params string[] parameters
            )
        {
            // can't validate anything for the type of a null
            if (node is not null)
            {
                // While this is not a comprehensive list. it covers the most common mistakes directly
                Assert.IsNotInstanceOfType<Compilation>(node, message, parameters);
                Assert.IsNotInstanceOfType<ISymbol>(node, message, parameters);
                Assert.IsNotInstanceOfType<SyntaxNode>(node, message, parameters);
            }
        }

        /// <summary>
        /// Extension method for use with <see cref="Assert.That"/> to validate that all <see cref="IncrementalGeneratorRunStep.Outputs"/>
        /// are either <see cref="IncrementalStepRunReason.Cached"/> or <see cref="IncrementalStepRunReason.Unchanged"/>
        /// </summary>
        /// <param name="_">Unused, provides <see cref="Assert.That"/> extension support</param>
        /// <param name="tuples"></param>
        /// <param name="stepTrackingName"></param>
        public static void OutputsCachedOrUnchanged(
            this Assert _,
            IncrementalGeneratorRunStep runStep,
            string stepTrackingName
            )
        {
            if (runStep is null)
            {
                throw new ArgumentNullException(nameof(runStep));
            }

            Assert.IsFalse(
                runStep.Outputs.Any(x => x.Reason != IncrementalStepRunReason.Cached && x.Reason != IncrementalStepRunReason.Unchanged),
                $"{stepTrackingName} should have only cached or unchanged reasons!"
                );
        }

        /// <summary>
        /// Extension method for use with <see cref="Assert.That"/> to validate that the output of a <see cref="IncrementalGeneratorRunStep"/>
        /// doesn't use any banned types.
        /// </summary>
        /// <param name="_"></param>
        /// <param name="runStep">Run step to validate</param>
        /// <param name="stepTrackingName"></param>
        /// <remarks>
        /// <note title="Ideally in an analyzer">
        /// It is debatable if this should be used in a test or an analyzer. In a test it is easy
        /// to omit from the tests (or not test at all in early development cycles).
        /// An analyzer can operate as you type code in the editor or when you compile the code so
        /// has a greater chance of catching erroneous use. Unfortunately no such analyzer exists
        /// as of yet. [It's actually hard to define the rules an analyzer should follow]. So this
        /// will do the best it can for now...</note>
        /// </remarks>
        static void ObjectGraphContainsValidSymbols(
            this Assert _,
            IncrementalGeneratorRunStep runStep,
            string stepTrackingName
            )
        {
            // Including the stepTrackingName in error messages to make it easier to isolate issues
            string because = "Step shouldn't contain banned symbols or non-equatable types. [{0}; {1}]";
            var visited = new HashSet<object>();

            // Check all of the outputs - probably overkill, but why not
            foreach (var (obj, _) in runStep.Outputs)
            {
                Visit(obj, visited, because, stepTrackingName, runStep.Name ?? "<null>");
            }

            // Private static function to recursively validate an object is cacheable
            static void Visit(
                object? node,
                HashSet<object> visitedNodes,
                /*[StringSyntax(StringSyntaxAttribute.CompositeFormat)]*/ string message,
                params string[] parameters
                )
            {
                // If we've already seen this object, or it's null, stop.
                if (node is null || !visitedNodes.Add(node))
                {
                    return;
                }

                Assert.That.NotBannedType(node, message, parameters);

                // Skip basic types and anything equatable, this includes
                // any equatable collections such as EquatableArray<T> as
                // that implies all elements are equatable already.
                Type type = node.GetType();
                if (type.IsBasicType() || type.IsEquatable())
                {
                    return;
                }

                // If the object is a collection, check each of the values
                if (node is IEnumerable collection and not string)
                {
                    foreach (object element in collection)
                    {
                        // recursively check each element in the collection
                        Visit(element, visitedNodes, message, parameters);
                    }
                }
                else
                {
                    // Recursively check each field in the object
                    foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        object? fieldValue = field.GetValue(node);
                        Visit(fieldValue, visitedNodes, message, parameters);
                    }
                }
            }
        }
    }
}