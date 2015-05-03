﻿using JetBrains.Annotations;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Relational;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using Microsoft.Data.Entity.Utilities;
using System.IO;

namespace ErikEJ.Data.Entity.SqlServerCe
{
    public class SqlServerCeDataStoreCreator : RelationalDataStoreCreator, ISqlServerCeDataStoreCreator
    {
        private readonly ISqlServerCeConnection _connection;
        private readonly IModelDiffer _modelDiffer;
        private readonly ISqlServerCeMigrationSqlGenerator _migrationSqlGenerator;
        private readonly ISqlStatementExecutor _executor;

        public SqlServerCeDataStoreCreator(
            [NotNull] ISqlServerCeConnection connection,
            [NotNull] IModelDiffer modelDiffer,
            [NotNull] ISqlServerCeMigrationSqlGenerator migrationSqlGenerator,
            [NotNull] ISqlStatementExecutor sqlStatementExecutor)
        {
            Check.NotNull(connection, nameof(connection));
            Check.NotNull(modelDiffer, nameof(modelDiffer));
            Check.NotNull(migrationSqlGenerator, nameof(migrationSqlGenerator));
            Check.NotNull(sqlStatementExecutor, nameof(sqlStatementExecutor));

            _connection = connection;
            _modelDiffer = modelDiffer;
            _migrationSqlGenerator = migrationSqlGenerator;
            _executor = sqlStatementExecutor;
        }

        public override void Create()
        {
            _connection.CreateDatabase();
        }

        public override void CreateTables(IModel model)
        {
            //TODO - MIgrations
            //    Check.NotNull(model, nameof(model));

            //    var operations = _modelDiffer.GetDifferences(null, model);
            //    var statements = _migrationSqlGenerator.Generate(operations, model);
            //    _executor.ExecuteNonQuery(_connection, null, statements);
        }

        public override bool Exists()
        {
            return _connection.Exists();
        }        

        public override bool HasTables()
        {
            var count = (long)_executor.ExecuteScalar(
                _connection,
                null,
                "SELECT COUNT(*) FROM INFORAMTION_SCHEMA.TABLES WHERE TABLE_TYPE <> N'SYSTEM TABLE';");

            return count != 0;
        }

        public override void Delete()
        {
            _connection.Delete();
        }
    }
}
