#!/bin/bash

# Setting Path and File location
checksumDirectory=$1
checksumListFileName=$2
checksumFileName=$3

# Generate a SHA-256 hash per file recursively
echo "---------------------------------------------"
echo "Starting Checksum Script..."
echo "---------------------------------------------"
echo "Creating 'Checksum' directory: '$checksumDirectory'"
mkdir -p $checksumDirectory || { echo "ERROR Creating Directory $checksumDirectory"; exit 1; }
echo "Generating SHA-256 hash per file under directory: '$PWD'"
echo "Checksum List will be stored to: '$checksumDirectory$checksumListFileName'"
echo "Starting generating SHA-256 hash..."
echo "---------------------------------------------"
cat $(find . -type f -exec sha256sum "{}" + > $checksumDirectory$checksumListFileName) $checksumDirectory$checksumListFileName || { echo "ERROR: Failed generating checksums"; exit 1; }

if [ -n "$checksumFileName" ]
then
	echo "Checksum will be stored to: '$checksumDirectory$checksumFileName'"
	sha256sum $checksumDirectory$checksumListFileName | awk '{print $1}' > $checksumDirectory$checksumFileName 
	echo "Checksum is $(cat $checksumDirectory$checksumFileName)"
fi

echo "---------------------------------------------"
echo "Checksum script completed."
echo "---------------------------------------------"