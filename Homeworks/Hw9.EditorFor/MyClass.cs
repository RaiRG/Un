using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SQLitePCL;

namespace Hw9.EditorFor
{
    public class Node
    {
        public PropertyInfo current;
        public List<Type> previous;

        public Node(PropertyInfo property)
        {
            current = property;
            previous = new List<Type>();
        }
    }

    public static class MyClass
    {
        public static IHtmlContent MyEditorFor(this IHtmlHelper helper, object obj)
        {
            if (obj == null)
                throw new Exception("Не может быть пустым");
            //
            //
            // visitedTypes.Add(obj.GetType().Name);
            var properties = obj.GetType().GetProperties();
            var builder = new HtmlContentBuilder();
            var dictionary = new Dictionary<PropertyInfo, List<Type>>();
            var previousTypes = new List<Type>();
            previousTypes.Add(obj.GetType());
            var previous = obj.GetType();
            var isFirst = true;
            foreach (var property in properties)
            {
                dictionary.Add(property, previousTypes);
                
                if (property.PropertyType.IsClass && property.PropertyType.Name != "String")
                {
                    if (dictionary[property].Contains(property.PropertyType))
                    {
                        throw new Exception("Невозможно использовать для вложенных объектов.");
                    }
                    
                    var add = MyEditorFor(helper, property.GetValue(obj));
                    builder.AppendHtml(add);
                }
                else
                {
                    var type = property.PropertyType;
                    builder.AppendHtml(
                        @$"<div class=""editor-label""><label for=""{property.Name}"">{property.Name}</label></div>");
                    switch (type.Name)
                    {
                        case "Int32":
                            builder.AppendHtml(GetInt(property, obj));
                            break;
                        case "String":
                            builder.AppendHtml(GetString(property, obj));
                            break;
                        case "Int64":
                            builder.AppendHtml(GetLong(property, obj));
                            break;
                        case "Boolean":
                            builder.AppendHtml(GetBool(property, obj));
                            break;
                        default:
                            if (type.BaseType == typeof(Enum))
                                builder.AppendHtml(GetString(property, obj));
                            break;
                    }
                }
            }

            return builder;
        }

        private static IHtmlContent GetString(PropertyInfo property, object? obj)
        {
            return new HtmlContentBuilder()
                .AppendHtml(
                    @$"<div class=""editor-field""><input class=""text-box single-line"" id=""{property.Name}"" name=""{property.Name}"" type=""text"" value=""{property.GetValue(obj).ToString()}""> <span class=""field-validation-valid"" data-valmsg-for=""{property.Name}"" data-valmsg-replace=""true""></span></div>");
        }

        private static IHtmlContent GetBool(PropertyInfo property, object? obj)
        {
            var result = new HtmlContentBuilder();
            if ((bool) property.GetValue(obj) == true)
                result.AppendHtml(
                    @$"<div class=""editor-field""><input checked=""checked"" class=""check-box"" data-val=""true"" data-val-required=""The Bool field is required."" id=""{property.Name}"" name=""{property.Name}""  type=""checkbox"" value=""true""><input name=""{property.Name}"" type=""hidden"" value=""false""> <span class=""field-validation-valid"" data-valmsg-for=""{property.Name}"" data-valmsg-replace=""true""></span></div>");
            else
            {
                result.AppendHtml(
                    $@"<div class=""editor-field""><input class=""check-box"" data-val=""true"" data-val-required=""The Bool field is required."" id=""{property.Name}"" name=""{property.Name}"" type=""checkbox"" value=""true""><input name=""{property.Name}"" type=""hidden"" value=""false""> <span class=""field-validation-valid"" data-valmsg-for=""{property.Name}"" data-valmsg-replace=""true""></span></div>");
            }

            return result;
        }

        private static IHtmlContent GetInt(PropertyInfo property, object obj)
        {
            return new HtmlContentBuilder()
                .AppendHtml(
                    @$"<div class=""editor-label""><input class=""text-box single-line"" data-val=""true"" data-val-required=""The Int field is required."" id=""{property.Name}"" name=""{property.Name}"" type=""number"" value=""{property.GetValue(obj)}""></div>");
        }

        private static IHtmlContent GetLong(PropertyInfo property, object obj)
        {
            return new HtmlContentBuilder()
                .AppendHtml(
                    @$"<div class=""editor-label""><input class=""text-box single-line"" data-val=""true"" data-val-required=""The Int field is required."" id=""{property.Name}"" name=""{property.Name}"" type=""number"" value=""{property.GetValue(obj)}""></div>");
        }
    }
}