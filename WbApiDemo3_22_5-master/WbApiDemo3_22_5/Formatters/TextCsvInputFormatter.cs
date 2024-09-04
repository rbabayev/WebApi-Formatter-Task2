using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using WbApiDemo3_22_5.Dtos;

namespace WbApiDemo3_22_5.Formatters
{
    public class TextCsvInputFormatter : TextInputFormatter
    {

        public TextCsvInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(StudentDto);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            HttpRequest request = context.HttpContext.Request;
            using StreamReader streamReader = new(request.Body, encoding);
            try
            {
                string data = await streamReader.ReadToEndAsync();
                var parts = data.Split('-');
                if (!data.StartsWith("Id - Fullname - SeriaNo - Age - Score"))
                {
                    string errorMessage = "Wrong Format!";
                    context.ModelState.TryAddModelError(context.ModelName, errorMessage);
                    throw new Exception(errorMessage);
                }

                StudentDto dto = new()
                {
                    Id = int.Parse(parts[0].Trim()),
                    Fullname = parts[1].Trim(),
                    SeriaNo = parts[2].Trim(),
                    Age = int.Parse(parts[3].Trim()),
                    Score = int.Parse(parts[4].Trim()),
                };

                return await InputFormatterResult.SuccessAsync(dto);
            }
            catch
            {
                return await InputFormatterResult.FailureAsync();
            }
        }
    }
}
