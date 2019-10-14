#!/bin/bash

DRY_RUN=false;

# show usage
function usage() {
  if [ -n "$1" ]; then
  echo "";
  echo -e "${RED}:point_right: $1${CLEAR}\n";
  fi
  echo "Usage: $0 [-s source] [-k api-key] [-d dry-run] packages"
  echo "";
  echo "Pushes nupkg packages to source"
  echo ""
  echo "Options:"
  echo "  -h, --help            Prints out a short help for the command."
  echo "  -s, --source          Specifies the server URL."
  echo "  -k, --api-key         The API key for the server."
  echo "  -d, --dry-run         Dry runs the command without executing it."
  echo ""
  echo "Example: $0 -source http://www.nuget.org --api-key secret -d /somepath/*.1.0.0.nupkg /somepath/*.1.0.0.symbols.nupkg"
  exit 1
}

#push Packages
function dotnet_publish() {
   if [ -f "$1" ]; then
       echo "Pushing package $1..."
     DOTNET_PUSH_CMD="dotnet nuget push --source $SOURCE --api-key $API_KEY $1"
     if $DRY_RUN; then echo "[dry run]: $DOTNET_PUSH_CMD"; else $DOTNET_PUSH_CMD; fi;
   else
     echo "Nuget package "$1" not found ...skipping";
   fi
}

# parse params
while [[ "$#" > 0 ]]; do case $1 in
  -h|--help) usage; shift;;
  -s|--source) SOURCE="$2";shift;shift;;
  -k|--api-key) API_KEY="$2";shift;shift;;
  -d|--dry-run) DRY_RUN=true; echo -e "Running in DRY RUN mode - Nuget packaged will not be pushed...\n"; shift;;
  --) # End of all options
      echo "end:";
      shift
      break;
      ;;
  -*)
      echo "Error: Unknown option: $1" >&2
      exit 1
      ;;
  *) PACKAGES=(${@});
  break
      ;;
esac; done

# verify params
if [ -z "$SOURCE" ]; then usage "Source (--source) is not set."; fi;
if [ -z "$API_KEY" ]; then usage "API Key (--api-key) is not set."; fi;
if [ -z "$PACKAGES" ]; then usage "No packages to push specified"; fi;

for file in "${PACKAGES[@]}"; do [[ $file != *.nupkg ]] && usage "Invalid argument specified: $file"; done

for file in "${PACKAGES[@]}"; do dotnet_publish $file; done