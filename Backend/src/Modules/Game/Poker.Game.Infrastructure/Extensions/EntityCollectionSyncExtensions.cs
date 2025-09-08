namespace Poker.Game.Infrastructure.Extensions;

public static class EntityCollectionSyncExtensions
{
    public static void SyncCollection<TChild, TKey>(
        this ICollection<TChild> dbCollection,
        IEnumerable<TChild> incomingCollection,
        Func<TChild, TKey> keySelector,
        Action<TChild, TChild> updateAction,
        Action<TChild>? addAction = null,
        Action<TChild>? onRemove = null,
        Func<TChild, bool>? filterForRemoval = null
    ) where TChild : class where TKey : notnull
    {
        var incomingDict = incomingCollection.ToDictionary(keySelector);
        var dbItems = dbCollection.ToList();

        foreach (var dbItem in dbItems)
        {
            var key = keySelector(dbItem);
            if (incomingDict.TryGetValue(key, out var incomingItem))
            {
                updateAction(dbItem, incomingItem);
            }
            else
            {
                if (filterForRemoval == null || filterForRemoval(dbItem))
                {
                    onRemove?.Invoke(dbItem);
                    dbCollection.Remove(dbItem);
                }
            }
        }

        foreach (var incomingItem in incomingCollection)
        {
            var key = keySelector(incomingItem);
            if (dbItems.All(x => key?.Equals(keySelector(x)) != true))
            {
                addAction?.Invoke(incomingItem);
                dbCollection.Add(incomingItem);
            }
        }
    }
}