![](https://i.imgur.com/pqhDLA8.png)

**Other Worlds** is a story/planet pack combo that adds a **new star system** to Kerbal Space Program full of **custom-made planets**, as well as a **custom-made story**, first in KSP history.

You can watch the [**trailer here**](https://www.youtube.com/watch?v=2L4yKnbbOfs)

## Requirements and Optional Mods

To play with this mod, you require to also install [**Kopernicus**](https://github.com/Kopernicus/Kopernicus/releases)

The mod also requires, but is already bunbled with, [VertexMitchellNetravaliHeightMap](https://github.com/pkmniako/Kopernicus_VertexMitchellNetravaliHeightMap) and [CTTP](https://github.com/Galileo88/Community-Terrain-Texture-Pack).

As optional mods, you can use *Enviromental Visual Enhancments*, *Scatterer* and *Parallax* for a more visually striking collection of planets.

You can also install [**Contract Configurator**](https://github.com/jrossignol/ContractConfigurator/releases) for a small collection of custom contracts.

## Other Source code

This mod also includes some laser sail parts that require custom code to work. You can find that one [here](https://github.com/pkmniako/Other_Worlds-Reboot).

## Changelog

- **1.0.5**
	- Story mode
		- Some signals' ranges made bigger
	- Fixes	
		- Applied new kopernicus settings for OnDemand loading and handling of far away colliders
		- Fixed generated waypoints with the custom UI missing a space
		- Rise range of sprite core antenna to reach the star (Between 2017 and now something happened)
		- Fixed Vassa not having clouds when using newer version of the Volumetric Clouds mod
- **1.0.4**
	- Celestial Bodies
		- Started testing EVE Raymarched clouds in Vassa if that version of EVE is installed.
	- Fixes
		- Module Manager applying clouds to every single EVE_CLOUDS node, ending up duplicating clouds, has been fixed.
		- Added template to Ice Giant to add missing settings which lack were breaking the RnD science archive screen.
- **1.0.3**
	- Story Mode
		- One signal's range made bigger
		- Made a clue slightly better
	- Fixes
		- Fixed lights from vessels showing up when reading signals
- **1.0.2**
	- Story Mode
		- Added a pop-out window on new saves to indicate what to do to start the story
		- Added other pop-outs
		- Rephrased a clue due to imprecision
	- Fixes
		- Lower the terrain quality of C4-2 to avoid some clipping.
- **1.0.1**
	- Fixes
		- Made parallax configuration for C2-1 less laggy
		- Partly removed kopernicus debugging things
- **1.0.0**
	- Story Mode
		- Added a story mode to the mod
	- Celestial Bodies
		- Usage of a [smoother heightmap PQS mod](https://github.com/pkmniako/Kopernicus_VertexMitchellNetravaliHeightMap)
		- Complete Redesign of [Troni](https://niakotheduck.blogspot.com/2022/04/a-lava-planet-redesign-of-troni.html)
		- Complete Redesign of [Kevari](https://niakotheduck.blogspot.com/2022/06/a-world-of-sulphur-and-heat-redesign-of.html)
		- New look for Prima and Secunda
		- New look for Nienna
		- Addition of new features to Pequar
		- Colormap changes for Vassa (Some regions are not on ice)
		- Colormap changes for C2-1 (Reintrodution of Green Substrate)
		- Biomes for C2-1
		- Biomes for C3-1
		- Biomes for Secunda
		- New Biomes for Troni
		- New Biomes for Prima
		- Added a lot of science experiment definitions (Courtesy of DibzNr)
	- Gameplay
		- Changed the Science points obtained from experiments
		- Adjusted contract weights so no exoplanet appears as random stock contracts
	- EVE (Clouds)
		- New Aurora for Pequar
		- Sand stream for Pequar
		- Made Scatter-less Atmospheric effects less powerful
	- Fixes
		- Biomes now have fixed internal names
		- Prima and Secunda now have more predictable vessel paths
		- C2-1 and C3-1 made smaller to have big, non-buggy Spheres of Influence
		- Reduced terrain quality on C2-1, reducing kraken attacks
		- Removed some seams on some cloud maps
	- Other
		- Most textures now use the OnDemand system, though not all

- **0.5.0 Beta**
	- Celestial Bodies
		- Moved Cercani to its spot in IC 
		- Cercani is now at a distance of 3.2 ki (3.2*10^13 m) away from home
		- Added asteroidal moon C4-1 orbiting Nienna
		- Added asteroidal moon C4-2 orbiting Nienna
		- Revamped Niko's map to have a better fitting look
		- Nienna's moons' orbits have been tweaked
		- Decreased the mass of Crons
		- Prima and Secunda are now proper binaries
		- Localization of planet's descriptions and names (French and Russian)
		- Non-Scatterer atmospheric effect if only using EVE (Credits go to JadeOfMaar)
		- Updated Atmospheres of all the planets. Now these work with Sigma Dimensions
		- Added Orbital Icons that were teased with the implementation of the feature
		- Changed Cercani's Rotation period
		- Changed Pequar's Surface Pressure to 0.2 atm
		- Changed Disole's Surface Pressure to 1.7 atm
		- Refined C2-1's and C3-1's color textures
	- Gameplay
		- Medium sized Laser Sails
		- Laser Generator
		- Easy to use UI
		- Ability to slow down a sail with lasers
		- Addition of ContractConfigurator contracts for exploration of interstellar space
		- Ore, CRP Resources and Classic Resources Distribution on all planets
	- Fixes
		- Fixed Scatterer-OnDemand loading bug. Even faster loading.
		- Fixed Non-Scatterer flare
		- Fixed Cercani's size and mass problem
		- Fixed strage sphere orbiting Nienna
		- Fixed Atmospheric Rims on some of the planets
		- Fixed weird coloring on some planets' surfaces
		- Fixed some biome maps
		- Fixed some real-scaled space transitions
	- Other
		- French/Spanish/Russian translation of Part Descriptions/Names & Planet Descriptions/Biomes (Thanks to Arthur (FR) and Zarbon (RU) for their respective translations!)
		- Compatibility with Interstellar Consortium
		- Better organization of folders
		- Fixed possible cause of incompatibility with SVE
- **0.4.3 Alpha**
	- Added OnDemand loading. Less RAM usage
	- Fixed C2-1 and C3-1, now you can actually land on them!
	- Changed Vassa's Colormap, now with very nice gradients between grey and white
	- Changed Disole's Cliff color, now darker than the surroundings
	- Fixed Troni's biomes. More fixes for Troni will come soon
- **0.4.2 Alpha**
	- Finished Crons Color and Height maps (Finalized design)
	- Transformed most planet textures into .dds -> Faster Loading!
	- Added Compatibility with Kopernicus Expansion: Troni's lava glows on the dark
	- Biomes Maps for Crons and Troni (Functionality in later not yet implemented)
	- New Orbits for the planets, very similar but balanced, mainly inclinations
	- Removed weird polar asteroid belt around Cercani
	- Added proper asteroid belts around Cercani (Inner, Middle and Outer/Scattered)
- **0.4.1 Alpha**
	- 1.4.3 Compatible
	- Fixed ground textures and shininess
	- Added cirrus clouds to Nienna
	- Redid Inter-Vision's Flag. Now it doesn't say "Ointer". And OWR's flag too...
- **0.4 Alpha**
	- Balanced the part costs of the laser sail, so now it makes more sense in career mode!
	- Put the parts of the laser sail in the correct spots in the RnD Tech Tree
	- Added Kevari, Crons, Niko, Prima and Secunda, though not in their best states.
	- Added aurorae to Pequar (Currntly green, may change color to red)
	- Added a RCS pack for the sail, so to manouver better and pinpoint flybies (Needs some work)
	- Now OWR uses CTTP v1.0.3 onwards
	- SigmaLoadingScreens compatible
- **0.3 Alpha**
	- Added Nienna
	- Added the SPRITE parts (Project Breakthrough Starshot Laser Sail)
	- Shenanigans with Disole's cloud colors
	- Tweaked Cercani's Sunflare
- **0.2 Alpha**
	- Now for 1.3.1
	- Cercani was moved 10 times closed to Kerbol
	- Pequar, Disole and C3-1 are added.
	- Removed EVE shadows compatibility due to bugs.
- **0.1.1 Alpha**
	- Added Troni... again.
- **0.1 Alpha**
	- Added Cercani
	- Not Added Troni
	- Added Vassa
	- Added C2-1

## Licensing

The [license](License.txt) for this mod comes in two parts:
- C# Code (That found on the src folder on github) is licensed under **MIT**
- Character dialogue and everything else falls under **ARR**
- Check [the credits](#Credits) for any additional assets used that may be distributed under a different license

## Credits
- Those who worked directly in the mod:
	- [Beale](https://forum.kerbalspaceprogram.com/profile/70533-beale/): Texture detail for a specific model
	- [DibzNr](https://forum.kerbalspaceprogram.com/profile/212219-dibznr/): Most of the science experiment descriptions
	- [JadeOfMaar](https://forum.kerbalspaceprogram.com/profile/167617-jadeofmaar/): Modded resource files
- Redistributed dependencies
	- [CTTP](https://github.com/Galileo88/Community-Terrain-Texture-Pack) under **Attribution-NonCommercial-ShareAlike 4.0 International**
	- [Kopernicus_VertexMitchellNetravaliHeightMap](https://github.com/pkmniako/Kopernicus_VertexMitchellNetravaliHeightMap) under **MIT**
- Local height terrain data:
	- [Mapzen](https://www.mapzen.com/) Terrain Tiles
	- NASA's [LOLA](https://science.nasa.gov/mission/lro/lola/) height data
- [One terrain texture and its normal](gamedata/OtherWorldsReboot/Bundles/ISLAND_CLR.png) by [Rob Tuytel](https://polyhaven.com/a/rocks_ground_06), redistributed under **CC0**
- Bunbled font [Nunito](https://fonts.google.com/specimen/Nunito/about) redistributed under [Open Font License](https://openfontlicense.org/)
- Most ring textures use highly modified ring textures from [SpaceEngine](https://spaceengine.org/)