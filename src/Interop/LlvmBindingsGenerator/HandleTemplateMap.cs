// -----------------------------------------------------------------------
// <copyright file="HandleTemplateMap.cs" company="Ubiquity.NET Contributors">
// Copyright (c) Ubiquity.NET Contributors. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using LlvmBindingsGenerator.Templates;

namespace LlvmBindingsGenerator
{
    /// <summary>Maps a type of Handle via it's native name (as a string) to a template that will generate code for it</summary>
    /// <remarks>
    /// This is generally obsolete, the new use of a source generator for handles now means that they are all listed in one
    /// of two files (GlobalHandles.cs or ContextHandles.cs) so the entire plan here of a single template is broken.
    /// </remarks>
    internal class HandleTemplateMap
        : KeyedCollection<string, IHandleCodeTemplate>
        , IReadOnlyDictionary<string, IHandleCodeTemplate>
    {
        public IEnumerable<string> DisposeFunctionNames
            => from item in Items
               let ght = item as GlobalHandleTemplate
               where ght != null && !string.IsNullOrWhiteSpace( ght.HandleDisposeFunction )
               select ght.HandleDisposeFunction;

        public IEnumerable<string> Keys => Items.Select( GetKeyForItem );

        public IEnumerable<IHandleCodeTemplate> Values => Items;

        public bool ContainsKey(string key) => TryGetValue( key, out IHandleCodeTemplate _ );

        IEnumerator<KeyValuePair<string, IHandleCodeTemplate>> IEnumerable<KeyValuePair<string, IHandleCodeTemplate>>.GetEnumerator()
        {
            return Items.Select( i => new KeyValuePair<string, IHandleCodeTemplate>( GetKeyForItem( i ), i ) )
                        .GetEnumerator();
        }

        protected override string GetKeyForItem(IHandleCodeTemplate item) => item.HandleName;
    }
}
