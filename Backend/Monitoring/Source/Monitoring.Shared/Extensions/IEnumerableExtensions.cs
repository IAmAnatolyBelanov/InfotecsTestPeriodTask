namespace Monitoring.Shared.Extensions;

/// <summary>
/// Класс для методов-расширений для <see cref="IEnumerable{T}"/> и его наследников.
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>
    /// Определяет, есть ли в коллекции элементы.
    /// </summary>
    /// <typeparam name="T">Тип коллекции.</typeparam>
    /// <param name="collection">Коллекция.</param>
    /// <returns><see langword="false"/>, если коллекция равна <see langword="null"/> или она пуста. В противном случае <see langword="true"/>.</returns>
    public static bool HasSome<T>(this IReadOnlyCollection<T>? collection)
        => collection != null && collection.Count > 0;

    /// <summary>
    /// Определяет, есть ли в списке элементы.
    /// </summary>
    /// <typeparam name="T">Тип списка.</typeparam>
    /// <param name="collection">Список.</param>
    /// <returns><see langword="false"/>, если список равен <see langword="null"/> или он пуст. В противном случае <see langword="true"/>.</returns>
    public static bool HasSome<T>(this IEnumerable<T>? collection)
        => collection != null && collection.Any();

    /// <summary>
    /// Выполняет действие на каждый элемент коллекции.
    /// </summary>
    /// <typeparam name="T">Тип коллекции.</typeparam>
    /// <param name="collection">Коллекция.</param>
    /// <param name="action">Действие.</param>
    public static void ForEach<T>(this IReadOnlyList<T> collection, Action<T> action)
    {
        for (var i = 0; i < collection.Count; i++)
        {
            action(collection[i]);
        }
    }

    /// <summary>
    /// Выполняет действие на каждый элемент коллекции.
    /// </summary>
    /// <typeparam name="T">Тип коллекции.</typeparam>
    /// <param name="collection">Коллекция.</param>
    /// <param name="action">Действие.</param>
    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }
}
