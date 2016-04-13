using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCollectionOrderer("Orderer", "MessagePlexTests")]
