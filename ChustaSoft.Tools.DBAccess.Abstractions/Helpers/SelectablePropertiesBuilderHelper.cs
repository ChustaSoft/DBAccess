using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ChustaSoft.Tools.DBAccess
{
    public static class SelectablePropertiesBuilderHelper
    {

#pragma warning disable IDE0060 // Remove unused parameter
        public static SelectablePropertiesBuilder<TOrigin, TSelection> Including<TOrigin, TSelection>(this IQueryable<TOrigin> originObj, Expression<Func<TOrigin, TSelection>> navigationPropertyPath)
            where TOrigin : class
        {
            var propertyName = GetPropertyName(navigationPropertyPath);

            var builder = new SelectablePropertiesBuilder<TOrigin, TSelection>(propertyName);

            return builder;
        }
#pragma warning restore IDE0060 // Remove unused parameter

        public static SelectablePropertiesBuilder<TOrigin, TSelection> Including<TOrigin, TParent, TSelection>(this SelectablePropertiesBuilder<TOrigin, TParent> builder, Expression<Func<TOrigin, TSelection>> navigationPropertyPath)
            where TOrigin : class
        {
            var propertyName = GetPropertyName(navigationPropertyPath);

            return new SelectablePropertiesBuilder<TOrigin, TSelection>(builder, propertyName);
        }

        public static SelectablePropertiesBuilder<TOrigin, TNewSelection> Including<TOrigin, TParent, TSelection, TNewSelection>(this SelectablePropertiesBuilder<TOrigin, TParent, TSelection> builder, Expression<Func<TOrigin, TNewSelection>> navigationPropertyPath)
            where TOrigin : class
        {
            var propertyName = GetPropertyName(navigationPropertyPath);

            return new SelectablePropertiesBuilder<TOrigin, TNewSelection>(builder, propertyName);
        }

        public static SelectablePropertiesBuilder<TOrigin, TParent, TSelection> Then<TOrigin, TParent, TSelection>(this SelectablePropertiesBuilder<TOrigin, TParent> builder, Expression<Func<TParent, TSelection>> navigationPropertyPath)
            where TOrigin : class
            where TParent : class
        {
            var propertyName = GetPropertyName(navigationPropertyPath);

            return new SelectablePropertiesBuilder<TOrigin, TParent, TSelection>(builder, propertyName);
        }

        public static SelectablePropertiesBuilder<TOrigin, TParent, TSelection> Then<TOrigin, TParent, TSelection>(this SelectablePropertiesBuilder<TOrigin, IEnumerable<TParent>> builder, Expression<Func<TParent, TSelection>> navigationPropertyPath)
            where TOrigin : class
            where TParent : class
        {
            var propertyName = GetPropertyName(navigationPropertyPath);

            return new SelectablePropertiesBuilder<TOrigin, TParent, TSelection>(builder, propertyName);
        }

        public static SelectablePropertiesBuilder<TOrigin, TParent, TSelection> Then<TOrigin, TPreviousParent, TParent, TSelection>(this SelectablePropertiesBuilder<TOrigin, TPreviousParent, TParent> builder, Expression<Func<TParent, TSelection>> navigationPropertyPath)
            where TOrigin : class
            where TPreviousParent : class
        {
            var propertyName = GetPropertyName(navigationPropertyPath);
            var selectionBuilder = new SelectablePropertiesBuilder<TOrigin, TParent, TSelection>(builder);

            selectionBuilder.AddDeepen(typeof(TSelection), propertyName);

            return selectionBuilder;
        }

        public static SelectablePropertiesBuilder<TOrigin, TParent, TSelection> Then<TOrigin, TPreviousParent, TParent, TSelection>(this SelectablePropertiesBuilder<TOrigin, TPreviousParent, IEnumerable<TParent>> builder, Expression<Func<TParent, TSelection>> navigationPropertyPath)
            where TOrigin : class
            where TPreviousParent : class
        {
            var propertyName = GetPropertyName(navigationPropertyPath);
            var selectionBuilder = new SelectablePropertiesBuilder<TOrigin, TParent, TSelection>(builder);

            selectionBuilder.AddDeepen(typeof(TSelection), propertyName);

            return selectionBuilder;
        }

        public static SelectablePropertiesBuilder<TOrigin, TParent> And<TOrigin, TParent, TSelection>(this SelectablePropertiesBuilder<TOrigin, TParent> builder, Expression<Func<TParent, TSelection>> navigationPropertyPath)
        {
            var propertyName = GetPropertyName(navigationPropertyPath);

            builder.AddFlush(typeof(TSelection), propertyName, false);

            return builder;
        }

        public static SelectablePropertiesBuilder<TOrigin, TParent, TPreviousSelection> And<TOrigin, TParent, TPreviousSelection, TSelection>(this SelectablePropertiesBuilder<TOrigin, TParent, TPreviousSelection> builder, Expression<Func<TParent, TSelection>> navigationPropertyPath) 
        {
            var propertyName = GetPropertyName(navigationPropertyPath);

            builder.AddFlush(typeof(TSelection), propertyName, false);

            return builder;
        }

        public static SelectablePropertiesBuilder<TOrigin, TParent, IEnumerable<TPreviousSelection>> And<TOrigin, TParent, TPreviousSelection, TSelection>(this SelectablePropertiesBuilder<TOrigin, TParent, IEnumerable<TPreviousSelection>> builder, Expression<Func<TParent, TSelection>> navigationPropertyPath)
        {
            var propertyName = GetPropertyName(navigationPropertyPath);

            builder.AddFlush(typeof(TSelection), propertyName, false);

            return builder;
        }


        private static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            var member = (MemberExpression)navigationPropertyPath.Body;
            var propertyName = member.Member.Name;

            return propertyName;
        }

    }
}
