﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Imaging.Interop;
using Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.Snapshot;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Tree.Dependencies.Models
{
    internal sealed class TargetDependencyViewModel : IDependencyViewModel
    {
        private static ImmutableDictionary<string, ProjectTreeFlags> s_configurationFlags = ImmutableDictionary<string, ProjectTreeFlags>.Empty.WithComparers(StringComparer.Ordinal);

        private readonly bool _hasUnresolvedDependency;

        public TargetDependencyViewModel(ITargetFramework targetFramework, bool hasReachableVisibleUnresolvedDependency)
        {
            Caption = targetFramework.FriendlyName;
            Flags = GetCachedFlags(targetFramework);
            _hasUnresolvedDependency = hasReachableVisibleUnresolvedDependency;

            static ProjectTreeFlags GetCachedFlags(ITargetFramework targetFramework)
            {
                return ImmutableInterlocked.GetOrAdd(
                    ref s_configurationFlags,
                    targetFramework.FullName,
                    fullName => DependencyTreeFlags.DependencyConfigurationGroup.Add($"$TFM:{fullName}"));
            }
        }

        public string Caption { get; }
        public string? FilePath => null;
        public string? SchemaName => null;
        public string? SchemaItemType => null;
        public int Priority => GraphNodePriority.FrameworkAssembly;
        public ImageMoniker Icon => _hasUnresolvedDependency ? ManagedImageMonikers.LibraryWarning : KnownMonikers.Library;
        public ImageMoniker ExpandedIcon => _hasUnresolvedDependency ? ManagedImageMonikers.LibraryWarning : KnownMonikers.Library;
        public ProjectTreeFlags Flags { get; }
        public IDependency? OriginalModel => null;
    }
}
