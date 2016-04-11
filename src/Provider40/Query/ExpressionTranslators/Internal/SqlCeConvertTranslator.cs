﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class SqlCeConvertTranslator : IMethodCallTranslator
    {
        private static readonly Dictionary<string, DbType> _typeMapping = new Dictionary<string, DbType>
        {
            [nameof(Convert.ToByte)] = DbType.Byte,
            [nameof(Convert.ToDecimal)] = DbType.Decimal,
            [nameof(Convert.ToDouble)] = DbType.Double,
            [nameof(Convert.ToInt16)] = DbType.Int16,
            [nameof(Convert.ToInt32)] = DbType.Int32,
            [nameof(Convert.ToInt64)] = DbType.Int64,
            [nameof(Convert.ToString)] = DbType.String,
        };

        private static readonly List<Type> _supportedTypes = new List<Type>
        {
            typeof(bool),
            typeof(byte),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(long),
            typeof(short),
            typeof(string),
        };

        private static readonly IEnumerable<MethodInfo> _supportedMethods;

        static SqlCeConvertTranslator()
        {
            _supportedMethods = _typeMapping.Keys
                .SelectMany(t => typeof(Convert).GetTypeInfo().GetDeclaredMethods(t)
                    .Where(m => (m.GetParameters().Length == 1)
                        && _supportedTypes.Contains(m.GetParameters().First().ParameterType)));
        }

        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
            if (_supportedMethods.Contains(methodCallExpression.Method))
            {
                var arguments = new[] { Expression.Constant(_typeMapping[methodCallExpression.Method.Name]), methodCallExpression.Arguments[0] };

                return new SqlFunctionExpression("CONVERT", methodCallExpression.Type, arguments);
            }

            return null;
        }
    }
}
