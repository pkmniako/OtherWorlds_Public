# Get version from assembly file
ASSEMBLY_INFO_PATH="src/OtherWorldsProgram/Properties/AssemblyInfo.cs"
REGEX_VERSION_PARSING="\[assembly: KSPAssembly\(\"(.*)\",\s?([0-9]*),\s?([0-9]*),\s?([0-9]*)\)\]"
REGEX_VERSION_PARSING="\[assembly: KSPAssembly\(\"(.*)\", ([0-9]*), ([0-9]*), ([0-9]*)"
PATH_ORIGINAL=$(pwd)

while IFR= read -r line
do
	if [[ $line =~ $REGEX_VERSION_PARSING ]]
	then 
		V0="${BASH_REMATCH[2]}"
		V1="${BASH_REMATCH[3]}"
		V2="${BASH_REMATCH[4]}"
		break
	fi
done < "$ASSEMBLY_INFO_PATH"

PATH_ZIP=$PATH_ORIGINAL/builds/Other_Worlds_$V0.$V1.$V2.zip
echo "Version found: $V0.$V1.$V2"

# Copy important files
echo "Copying important files..."
cp -f README.md gamedata/OtherWorldsReboot/README.md
cp -f License.txt gamedata/OtherWorldsReboot/License.txt

# Zip mod
echo "Creating zip file at $PATH_ZIP, please wait..."
mkdir -p builds
cd gamedata
zip -r "$PATH_ZIP" *

# Zip dependencies
PATH_TO_GAMEDATA="/e/SteamLibrary/Kerbal Space Program - 1.12 - Planets/GameData"
cd "$PATH_TO_GAMEDATA"
zip -r $PATH_ZIP CTTP
zip -r $PATH_ZIP 000_NiakoUtils

# Done
cd "$PATH_ORIGINAL"
echo "Done building version $V0.$V1.$V2!"