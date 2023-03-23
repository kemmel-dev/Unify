﻿using System;
using System.Collections.Generic;

namespace Unify.Core
{
    public class UnifyContainer
    {
        private Dictionary<UnifyDependency, object> _localDependencies = new ();

        public void RegisterDependency<T>(object instance, string id = default)
        {
            var dependency = new UnifyDependency(typeof(T), id);
            if (_localDependencies.ContainsKey(dependency))
                throw new Exception("Already contains a dependency with the same key!");
            _localDependencies[dependency] = instance;
        }

        public object ResolveDependency(Type type, string id = default)
        {
            var dependency = new UnifyDependency(type, id);
            if (_localDependencies.ContainsKey(dependency))
            {
                return _localDependencies[dependency];
            }
            throw new Exception($"Cannot find a dependency with this key with type {type + (id == default ? "" : $"id {id}")}!");
        }


        public void RegisterDependenciesFrom(UnifyContainer monoInstallerLocalContainer)
        {
            var dependenciesInOtherContainer = monoInstallerLocalContainer._localDependencies;
            foreach (var dependencyObjectPair in dependenciesInOtherContainer)
            {
                if (_localDependencies.ContainsKey(dependencyObjectPair.Key))
                    throw new Exception($"Already contains a dependency with key {dependencyObjectPair.Key}");
                _localDependencies[dependencyObjectPair.Key] = dependencyObjectPair.Value;
            }
        }
    }
}