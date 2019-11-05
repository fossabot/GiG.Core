FROM igcproget.igc.zone/gig-core-docker/library/dotnet:3.0.100-sdk-0.1.0.15.e3885ad as base

COPY . .
RUN dotnet restore GiG.Core.sln
RUN dotnet build GiG.Core.sln /p:Version=$VERSION -c Release --no-restore

FROM base AS publish
ENTRYPOINT ["/scripts/publish.sh"]

FROM base AS test
ENTRYPOINT ["/scripts/test.sh"]
CMD ["-u", "-i", "-f", "Category=Unit|Category=Integration", "/src/tests/"]