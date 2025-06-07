// -----------------------------------------------------------------------
// <copyright file="DirectedGraphSerializer.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml.Linq;

using OpenSoftware.DgmlTools.Model;

namespace Kaleidoscope.Grammar.Visualizers
{
    // XML Serialization of DirectedGraph  (and child nodes) via the extension method WriteToFile()
    // uses reflection which is antithetical to AOT scenarios. So this rolls it out manually without
    // need of reflection.
    internal static class DirectedGraphSerializer
    {
        public static XDocument ToXml( this DirectedGraph self )
        {
            var doc = new XDocument { Declaration = new XDeclaration("1.0", "utf-8", "yes") };
            var rootNode = new XElement( DgmlNs + nameof(DirectedGraph)
                                       , new XAttribute(nameof(self.DataVirtualized), self.DataVirtualized)
                                       , self.Nodes.ToXml(ToXml, DgmlNs + nameof(self.Nodes))
                                       , self.Links.ToXml(ToXml, DgmlNs + nameof(self.Links))
                                       , self.Categories.ToXml(ToXml, DgmlNs + nameof(self.Categories))
                                       , self.Styles.ToXml(ToXml, DgmlNs + nameof(self.Styles))
                                       , self.Properties.ToXml(ToXml, DgmlNs + nameof(self.Properties))
                                       );

            rootNode.AddOptionalAttribute( self.Background, nameof( self.Background ) );

            if(self.ShouldSerializeGraphDirection())
            {
                rootNode.Add( new XAttribute( nameof( self.GraphDirection ), self.GraphDirection ) );
            }

            if(self.ShouldSerializeLayout())
            {
                rootNode.Add( new XAttribute( nameof( self.Layout ), self.Layout ) );
            }

            if(self.ShouldSerializeNeighborhoodDistance())
            {
                rootNode.Add( new XAttribute( nameof( self.NeighborhoodDistance ), self.NeighborhoodDistance ) );
            }

            doc.Add( rootNode );
            return doc;
        }

        internal static XElement ToXml( this Node self )
        {
            var retVal = new XElement( DgmlNs + nameof(Node)
                                     , self.CategoryRefs.ToXml(ToXml)
                                     );
            retVal.AddOptionalAttribute( self.Id, nameof( self.Id ) );
            retVal.AddOptionalAttribute( self.Label, nameof( self.Label ) );
            retVal.AddOptionalAttribute( self.Category, nameof( self.Category ) );
            retVal.AddOptionalAttribute( self.Group, nameof( self.Group ) );
            retVal.AddOptionalAttribute( self.Description, nameof( self.Description ) );
            retVal.AddOptionalAttribute( self.Reference, nameof( self.Reference ) );
            foreach(var kvp in self.Properties)
            {
                retVal.AddOptionalAttribute( kvp.Value, kvp.Key );
            }

            return retVal;
        }

        internal static XElement ToXml( this CategoryRef self )
        {
            var retVal = new XElement( DgmlNs + nameof(CategoryRef));
            retVal.AddOptionalAttribute( self.Ref, nameof( self.Ref ) );
            return retVal;
        }

        internal static XElement ToXml( this Link self )
        {
            var retVal = new XElement( DgmlNs + nameof(Link)
                                     , self.CategoryRefs.ToXml(ToXml)
                                     );

            retVal.AddOptionalAttribute( self.Source, nameof( self.Source ) );
            retVal.AddOptionalAttribute( self.Target, nameof( self.Target ) );
            retVal.AddOptionalAttribute( self.Category, nameof( self.Category ) );
            retVal.AddOptionalAttribute( self.StrokeThickness, nameof( self.StrokeThickness ) );
            retVal.AddOptionalAttribute( self.Visibility, nameof( self.Visibility ) );
            retVal.AddOptionalAttribute( self.Background, nameof( self.Background ) );
            retVal.AddOptionalAttribute( self.Stroke, nameof( self.Stroke ) );
            retVal.AddOptionalAttribute( self.Label, nameof( self.Label ) );
            retVal.AddOptionalAttribute( self.Description, nameof( self.Description ) );
            return retVal;
        }

        internal static XElement ToXml( this Category self )
        {
            var retVal = new XElement( DgmlNs + nameof(Category));
            retVal.AddOptionalAttribute( self.Id, nameof( self.Id ) );
            retVal.AddOptionalAttribute( self.Label, nameof( self.Label ) );
            retVal.AddOptionalAttribute( self.Background, nameof( self.Background ) );
            retVal.AddOptionalAttribute( self.Icon, nameof( self.Icon ) );
            return retVal;
        }

        internal static XElement ToXml( this Style self )
        {
            var retVal = new XElement( DgmlNs + nameof(Style)
                                     , self.Condition.ToXml(ToXml)
                                     , self.Setter.ToXml(ToXml)
                                     );

            retVal.AddOptionalAttribute( self.TargetType, nameof( self.TargetType ) );
            retVal.AddOptionalAttribute( self.GroupLabel, nameof( self.GroupLabel ) );
            retVal.AddOptionalAttribute( self.ValueLabel, nameof( self.ValueLabel ) );
            return retVal;
        }

        internal static XElement ToXml( this Condition self )
        {
            var retVal = new XElement( DgmlNs + nameof(Condition));
            retVal.AddOptionalAttribute( self.Expression, nameof( self.Expression ) );
            return retVal;
        }

        internal static XElement ToXml( this Setter self )
        {
            var retVal = new XElement( DgmlNs + nameof(Setter));
            retVal.AddOptionalAttribute( self.Property, nameof( self.Property ) );
            retVal.AddOptionalAttribute( self.Value, nameof( self.Value ) );
            retVal.AddOptionalAttribute( self.Expression, nameof( self.Expression ) );
            return retVal;
        }

        internal static XElement ToXml( this Property self )
        {
            var retVal = new XElement( DgmlNs + nameof(Property));
            retVal.AddOptionalAttribute( self.Id, nameof( self.Id ) );
            retVal.AddOptionalAttribute( self.DataType, nameof( self.DataType ) );
            retVal.AddOptionalAttribute( self.Label, nameof( self.Label ) );
            retVal.AddOptionalAttribute( self.Description, nameof( self.Description ) );
            return retVal;
        }

        // Handles skipping attribute if the value is null.
        internal static void AddOptionalAttribute( this XElement element, object? value, XName name )
        {
            if(value is not null && name is not null)
            {
                element.Add( new XAttribute( name, value ) );
            }
        }

        // NOTE: The list handling for an xml serializer is a generally under-documented "feature" that is VERY confusing.
        // Especially with regard to whether a "container" element is emitted or not. This tries to simplify things by
        // emitting a container element ONLY if containerElementName != null. Otherwise, this produces individual elements.
        internal static IEnumerable<XNode> ToXml<T>( this List<T> container, Func<T, XNode> serializer, XName? containerElementName = null )
        {
            if(container is null)
            {
                yield break;
            }

            if(containerElementName is null)
            {
                foreach(var child in container)
                {
                    yield return serializer( child );
                }
            }
            else
            {
                var containerElement = new XElement( containerElementName );
                foreach(var child in container)
                {
                    containerElement.Add( serializer( child ) );
                }

                yield return containerElement;
            }
        }

        // Namespace are a real PITA with XML LINQ. It insists on taking control and ignores the
        // standard XML functionality of inheriting namespaces. Instead it INSISTS that all elements
        // provide the ns name (and then eliminates them for inheritance). This makes for a lot of
        // tedious setting of the namespace only to have it removed again by the framework...
        private static readonly XNamespace DgmlNs = @"http://schemas.microsoft.com/vs/2009/dgml";
    }
}
