namespace Simple.Web.CodeGeneration
{
    using ContentTypeHandling;

    static class WriteView
    {
        public static void Impl(object handler, IContext context)
        {
            WriteUsingContentTypeHandler(handler, context);
        }

        private static void WriteUsingContentTypeHandler(object handler, IContext context)
        {
            IContentTypeHandler contentTypeHandler;
            if (TryGetContentTypeHandler(context, out contentTypeHandler))
            {
                context.Response.ContentType = contentTypeHandler.GetContentType(context.Request.AcceptTypes);
                var content = new Content(handler, null);
                contentTypeHandler.Write(content, context.Response.Output);
            }
        }

        private static bool TryGetContentTypeHandler(IContext context, out IContentTypeHandler contentTypeHandler)
        {
            try
            {
                contentTypeHandler = new ContentTypeHandlerTable().GetContentTypeHandler(context.Request.AcceptTypes);
            }
            catch (UnsupportedMediaTypeException)
            {
                context.Response.StatusCode = 415;
                context.Response.StatusDescription = "Unsupported media type requested.";
                contentTypeHandler = null;
                return false;
            }
            return true;
        }
    }
}