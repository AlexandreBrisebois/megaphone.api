FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS final
ENV INSTRUMENTATION_KEY="insert-key-here"
EXPOSE 80
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "megaphone.api.dll"]