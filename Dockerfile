FROM mcr.microsoft.com/mssql/server:2022-latest
ENV SA_PASSWORD=YourStrongPassword123
ENV ACCEPT_EULA=Y
EXPOSE 1433
COPY ./init-database.sql /init-database.sql
CMD /opt/mssql/bin/sqlservr & sleep 30 && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i /init-database.sql && wait