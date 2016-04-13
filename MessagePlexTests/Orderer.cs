using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Sdk;
using Xunit.Abstractions;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
class Orderer : Attribute, ITestCollectionOrderer, ITestCaseOrderer
{
    static readonly string fqName = typeof(Orderer).AssemblyQualifiedName;

    public double Order { get; set; }

    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        => testCases.OrderBy(test => test.Traits["Order"].SingleOrDefault());

    public IEnumerable<ITestCollection> OrderTestCollections(IEnumerable<ITestCollection> testCollections)
    => testCollections.OrderBy(test =>
    {
        var attrInfo = test.CollectionDefinition?.GetCustomAttributes(fqName).SingleOrDefault();
        if (attrInfo == null)
            return 0;
        return attrInfo.GetNamedArgument<double>("Order");
    });
}
