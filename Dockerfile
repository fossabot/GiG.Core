FROM igcproget.igc.zone/gig-common-docker/dotnet/sdk:3.1.101-0.1.0.5 as base

COPY . .
RUN dotnet restore GiG.Core.sln
RUN dotnet build GiG.Core.sln /p:Version=$VERSION -c Release --no-restore

FROM base AS push
ENTRYPOINT ["/scripts/push.sh"]

FROM base AS test
ENTRYPOINT ["/scripts/test.sh"]
CMD ["-u", "-i", "-f", "Category=Unit|Category=Integration|Category=Component", "/sln/tests"]