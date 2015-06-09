﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Relational.Query;
using Microsoft.Data.Entity.Relational.Query.Methods;
using Microsoft.Framework.Logging;

namespace ErikEJ.Data.Entity.SqlServerCe
{
    public class SqlCeCompositeMethodCallTranslator : RelationalCompositeMethodCallTranslator
    {
        //TODO ErikEJ Implement methods + add tests
        //private readonly List<IMethodCallTranslator> _sqlServerTranslators = new List<IMethodCallTranslator>
        //{
        //    new NewGuidTranslator(),
        //    new StringSubstringTranslator(),
        //    new MathAbsTranslator(),
        //    new MathCeilingTranslator(),
        //    new MathFloorTranslator(),
        //    new MathPowerTranslator(),
        //    new MathRoundTranslator(),
        //    new MathTruncateTranslator(),
        //    new StringReplaceTranslator(),
        //    new StringToLowerTranslator(),
        //    new StringToUpperTranslator(),
        //};

        public SqlCeCompositeMethodCallTranslator([NotNull] ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        //protected override IReadOnlyList<IMethodCallTranslator> Translators
        //    => base.Translators.Concat(_sqlServerTranslators).ToList();
    }
}
