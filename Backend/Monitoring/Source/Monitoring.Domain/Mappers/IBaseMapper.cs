namespace Monitoring.Domain.Mappers;

/// <summary>
/// Базовый интерфейс маппера.
/// </summary>
/// <typeparam name="TEntity">Тип объекта.</typeparam>
/// <typeparam name="TDto">Тип dto объекта.</typeparam>
public interface IBaseMapper<TEntity, TDto>
{
    /// <summary>
    /// Маппит <see cref="TEntity"/> к <see cref="TDto"/>.
    /// </summary>
    /// <param name="entity">Исходный объект, который нужно смаппить.</param>
    /// <returns>Новый объект, полученный в результате маппинга исходного.</returns>
    TDto MapToDto(TEntity entity);

    /// <summary>
    /// Маппит <see cref="TEntity"/> к <see cref="TDto"/>.
    /// </summary>
    /// <param name="entity">Исходный объект, который нужно смаппить.</param>
    /// <param name="dto">Объект, в который нужно произвести маппинг.</param>
    /// <returns>Если deviceInfoDto - null, то новый объект, полученный в результате маппинга исходного, иначе deviceInfoDto с изменениями в результате маппинга.</returns>
    TDto MapToDto(TEntity entity, TDto dto);

    /// <summary>
    /// Маппит <see cref="TDto"/> к <see cref="TEntity"/>.
    /// </summary>
    /// <param name="deviceInfoDto">Исходный объект, который нужно смаппить.</param>
    /// <returns>Новый объект, полученный в результате маппинга исходного.</returns>
    TEntity MapFromDto(TDto deviceInfoDto);

    /// <summary>
    /// Маппит <see cref="TDto"/> к <see cref="TEntity"/>.
    /// </summary>
    /// <param name="deviceInfoDto">Исходный объект, который нужно смаппить.</param>
    /// <param name="deviceInfo">Объект, в который нужно произвести маппинг.</param>
    /// <returns>Если deviceInfo - null, то новый объект, полученный в результате маппинга исходного, иначе deviceInfo с изменениями в результате маппинга.</returns>
    TEntity MapFromDto(TDto deviceInfoDto, TEntity deviceInfo);
}
