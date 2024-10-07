#!/usr/bin/env bash

# see http://redsymbol.net/articles/unofficial-bash-strict-mode/
set -euo pipefail
IFS=$'\n\t'
source ./CONFIG.inc

clean() {
	rm -fR $FILE
	if [ ! -d Archive ] ; then
		mkdir Archive
	fi
}

pwd=$(pwd)
FILE=${pwd}/Archive/$PACKAGE-$VERSION${PROJECT_STATE}-CurseForge.zip
echo $FILE
clean
cd GameData

zip -r $FILE ./ParkingBrake/* -x ".*"

#mkdir -p $pwd/bin/ParkingBrake
#cat $TARGETDIR/ParkingBrake.cfg | sed 's/curseforge_ready = False/curseforge_ready = True/g' > $pwd/bin/ParkingBrake/KSP-ParkingBrake.cfg
#pushd $pwd/bin
#zip -u $FILE ParkingBrake/ParkingBrake.cfg
#popd

zip -d $FILE __MACOSX "**/.DS_Store" || echo ""

set -e
cd $pwd
