# Get version from assembly file
ASSEMBLY_INFO_PATH="src/OtherWorldsProgram/Properties/AssemblyInfo.cs"
REGEX_VERSION_PARSING="\[assembly: KSPAssembly\(\"(.*)\",\s?([0-9]*),\s?([0-9]*),\s?([0-9]*)\)\]"
REGEX_VERSION_PARSING="\[assembly: KSPAssembly\(\"(.*)\", ([0-9]*), ([0-9]*), ([0-9]*)"

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

echo "Version found: $V0.$V1.$V2"

# Copy important files
cp -f README.md gamedata/OtherWorldsReboot/README.md
cp -f License.txt gamedata/OtherWorldsReboot/License.txt

# Zip
echo "Creating zip file, please wait..."
mkdir -p builds
cd gamedata
zip -r ../builds/Other_Worlds_$V0.$V1.$V2.zip *

# Done
echo "Done building version $V0.$V1.$V2!"