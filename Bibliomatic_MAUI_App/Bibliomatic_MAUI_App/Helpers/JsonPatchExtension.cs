using System.Text;

namespace Bibliomatic_MAUI_App.Helpers
{
    public enum PatchAction
    {
        Add,
        Replace,
        Remove,
        Move,
        Copy,
        Test
    }

    public class JsonPatchExtension
    {
        public static string CreateJsonPatchDocument(PatchAction patchAction, string property, object value)
        {
            string action = GetPatchAction(patchAction);
            string correctProperty = char.ToLower(property[0]) + property.Substring(1);
            string correctValue = GetValue(value);

            StringBuilder jsonPatchDocument = new StringBuilder();

            jsonPatchDocument.Append("[{");
            jsonPatchDocument.Append($"\"op\" : \"{action}\",");
            jsonPatchDocument.Append($"\"path\" : \"/{correctProperty}\",");
            jsonPatchDocument.Append($"\"value\" : {correctValue}");
            jsonPatchDocument.Append("}]");

            return jsonPatchDocument.ToString();
        }

        public static string CreateJsonPatchDocument(PatchAction patchAction, string property, Dictionary<string, object> values)
        {
            string action = GetPatchAction(patchAction);
            string correctProperty = char.ToLower(property[0]) + property.Substring(1);

            StringBuilder jsonPatchDocument = new StringBuilder();
            List<string> stringValues = new List<string>();

            jsonPatchDocument.Append("[{");
            jsonPatchDocument.Append($"\"op\" : \"{action}\",");
            jsonPatchDocument.Append($"\"path\" : \"/{correctProperty}/-\",");
            jsonPatchDocument.Append("\"value\" : {");

            foreach (var value in values)
            {
                string dataProperty = char.ToLower(value.Key[0]) + value.Key.Substring(1);
                string dataValue = GetValue(value.Value);
                stringValues.Add($"\"{dataProperty}\" : {dataValue}");
            }

            jsonPatchDocument.AppendJoin(',', stringValues);
            jsonPatchDocument.Append("}}]");

            return jsonPatchDocument.ToString();
        }

        private static string GetValue(object value)
        {
            if(value is bool boolValue)
            {
                if(boolValue)
                {
                    return "true";
                }

                return "false";
            }
            else if (value is int intValue)
            {
                return intValue.ToString();
            }

            return $"\"{value}\"";
        }

        private static string GetPatchAction(PatchAction patchAction)
        {
            string action = string.Empty;

            switch (patchAction)
            {
                case PatchAction.Add:
                    action = "add";
                    break;

                case PatchAction.Replace:
                    action = "replace";
                    break;

                case PatchAction.Remove:
                    action = "remove";
                    break;

                case PatchAction.Move:
                    action = "move";
                    break;

                case PatchAction.Copy:
                    action = "copy";
                    break;

                case PatchAction.Test:
                    action = "test";
                    break;

                default:
                    break;
            }

            return action;
        }
    }
}
