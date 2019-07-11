using MongoDB.Bson;
using MongoDB.Driver;
using OgreMage.API.Contracts.Database;
using OgreMage.API.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OgreMage.API.Services.Database
{
    /// <summary>
    /// Repositório padrão para acesso ao mongoDB.
    /// </summary>
    public class MongoRepository : IMongoRepository
    {
        private readonly IMongoConnection MongoConnection;

        public MongoRepository(IMongoConnection mongoConnection)
        {
            this.MongoConnection = mongoConnection;
        }

        /// <summary>
        /// Insere a model no mongoDB.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public TModel Insert<TModel>(TModel model)
            where TModel : class, IMongoModel
        {
            this.MongoConnection.GetCollection<TModel>().InsertOne(model);
            return model;
        }

        /// <summary>
        /// Altera a model com base em seu identificador.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public TModel Update<TModel>(TModel model)
            where TModel : class, IMongoModel
        {
            this.MongoConnection.GetCollection<TModel>().ReplaceOne(filter => filter.Id == model.Id, model);
            return model;
        }

        /// <summary>
        /// Consulta a model pelo indentificador informado.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public TModel FindById<TModel>(string id)
            where TModel : class, IMongoModel
        {
            return this.MongoConnection.GetCollection<TModel>()
                .Find(filter => filter.Id == new ObjectId(id)).FirstOrDefault();
        }

        /// <summary>
        /// Remove a model.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public TModel Remove<TModel>(TModel model)
            where TModel : class, IMongoModel
        {
            this.MongoConnection.GetCollection<TModel>().DeleteOne(filter => filter.Id == model.Id);
            return model;
        }

        /// <summary>
        /// Remove a model pelo identificador e retorna o documento removido.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public TModel Remove<TModel>(string id)
            where TModel : class, IMongoModel
        {
            return this.MongoConnection.GetCollection<TModel>()
                .FindOneAndDelete(filter => filter.Id == new ObjectId(id));
        }

        /// <summary>
        /// Consulta e filtra a model.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<TModel> Find<TModel>(Expression<Func<TModel, bool>> filter)
            where TModel : class, IMongoModel
        {
            return this.MongoConnection.GetCollection<TModel>()
                .Find(filter).ToList();
        }

        /// <summary>
        /// Consulta e projeta a model.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProjection"></typeparam>
        /// <param name="filter"></param>
        /// <param name="projection"></param>
        /// <returns></returns>
        public IEnumerable<TProjection> FindAndProject<TModel, TProjection>(
            Expression<Func<TModel, bool>> filter,
            Expression<Func<TModel, TProjection>> projection
            ) where TModel : class, IMongoModel
        {
            return this.MongoConnection.GetCollection<TModel>()
                .Find(filter).Project(projection).ToList();
        }

        /// <summary>
        /// Consulta, filtra e pagina a model.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<TModel> Find<TModel>(Expression<Func<TModel, bool>> filter, int page, int pageSize)
            where TModel : class, IMongoModel
        {
            return this.MongoConnection.GetCollection<TModel>()
                .Find(filter).Skip((page - 1) * pageSize)
                .Limit(pageSize).ToList();
        }

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
        public IEnumerable<TProjection> FindAndProject<TModel, TProjection>(
            Expression<Func<TModel, bool>> filter,
            Expression<Func<TModel, TProjection>> projection,
            int page, 
            int pageSize
            ) where TModel : class, IMongoModel
        {
            return this.MongoConnection.GetCollection<TModel>()
                .Find(filter).Project(projection)
                .Skip((page - 1) * pageSize).Limit(pageSize).ToList();
        }
    }
}
