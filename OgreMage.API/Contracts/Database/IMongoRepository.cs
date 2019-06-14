using MongoDB.Driver;
using OgreMage.API.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OgreMage.API.Contracts.Database
{
    /// <summary>
    /// Interface padrão para um repositório do mongoDB.
    /// </summary>
    public interface IMongoRepository
    {
        /// <summary>
        /// Insere a model no mongoDB.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        TModel Insert<TModel>(TModel model) where TModel : class, IMongoModel;

        /// <summary>
        /// Altera a model com base em seu identificador.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        TModel Update<TModel>(TModel model) where TModel : class, IMongoModel;

        /// <summary>
        /// Consulta a model pelo indentificador informado.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        TModel FindById<TModel>(string id) where TModel : class, IMongoModel;

        /// <summary>
        /// Remove a model.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        TModel Remove<TModel>(TModel model) where TModel : class, IMongoModel;

        /// <summary>
        /// Remove a model pelo identificador e retorna o documento removido.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        TModel Remove<TModel>(string id) where TModel : class, IMongoModel;

        /// <summary>
        /// Consulta e filtra a model.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<TModel> Find<TModel>(Expression<Func<TModel, bool>> filter) where TModel : class, IMongoModel;

        /// <summary>
        /// Consulta e projeta a model.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="filter"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        IEnumerable<TProjection> FindAndProject<TModel, TProjection>(Expression<Func<TModel, bool>> filter,
            Expression<Func<TModel, TProjection>> projection) where TModel : class, IMongoModel;

        /// <summary>
        /// Consulta, filtra e pagina a model.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IEnumerable<TModel> Find<TModel>(Expression<Func<TModel, bool>> filter, int page, int pageSize) where TModel : class, IMongoModel;

        /// <summary>
        /// Consulta, projeta e pagina a model.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="filter"></param>
        /// <param name="projection"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IEnumerable<TProjection> FindAndProject<TModel, TProjection>(Expression<Func<TModel, bool>> filter,
            Expression<Func<TModel, TProjection>> projection, int page, int pageSize) where TModel : class, IMongoModel;
    }
}
