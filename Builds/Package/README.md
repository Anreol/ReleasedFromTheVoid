# Released From The Void

A mod that re-implements unused Survivors From The Void content back into the game.

## Features
- Void Coins
- Void Suppressors.
	- Remove items from the run using Void Coins, removed items get converted into Strange Scrap.
	
- A new commando skin.
- Enabled a Lunar equipment
- Run
	- Void enemies can drop Void coins, 10% when killed
	- This chance gets increased to 30% in Void Locus

- Enabled unused enemies: Assassin & Major Construct
	- Assassin will spawn in Sundered Grove and Scorched Acres
	- Major Construct will spawn in Sky Meadows and Sulfur Pools, and by stage 3, it can appear in Golem Plains
	
## Balance Changes
- Bazaar
	- It will now have increased chance of Void Locus appearing on seers starting from stage 4
	- It will decrease back to normal chance once it is visited, for it to increase again after 4 stages.

- Void Locus
	- Will spawn 14 void barrels per player, up to four times.
	- Will spawn 2 void chests per player
	- Will spawn 1 triple chest per player
	- Interactables credits scale overtime, up to 520 (Same as Sky Meadows)
	- There's now a void portal available at all times to leave Void Locus
	- All chests inside now cost Void Coins.

- Enemies
	- Xi Construct will now start appearing in Golem Plains, Sulfur Pools, and Sky Meadows in stage 10
	- Changed as Major Construct is now added to the pool.
	- Assassin has been heavily altered from how it is in the base game. Just a few value changes.
	
- Void Cradles
	- Void Cradles purchased with Void Coins will not spawn Void Infestors.

## Known issues
Hopoo Issues:
Void Suppressor might get glitched if there was a triple void chest in the stage that has been used.
Commando's DLC skin has one leg missing whenever the player picks up a hoof.
Assassins have innacurate hitboxes, they are actually half of their body size.

About the mod itself, so far none, and I hope there's no networking bugs.
Void Camps, for now, have their interactables still cost health, as the Void Camp generation seems to be after all directors have populated the stage and the stage has started. (This might also the reason of why survivors spawn inside them. The Void Camp just spawns after the players)

### Contact
You can contact me by messaging to Anreol#8231 or @anreol:poa.st

## Changelog
**0.0.3**
- Finally updated
	- Added config to enable / disable the adding of spawn cards of Assassins and Major Constructs.
	- Added an extra rule so players can start with 10 void coins.
		- Changed instances of "Void Coins" to the proper "Void Marker". The first is just its internal name.
	- Major Construct
		- Gave it a name: Iota Construct, Surveillance System
		- Is now marked as Champion, as my implementation of it makes it a boss.
		- Gave it a unlockable so it shows in the logbook.
		- Added it to Sky Meadow Simulacrum.
	- Hopefully fixed the missing lunar item not showing up??
	- Some other stuff I probably forgot about.

**0.0.2**
- Balance Changes
	- Increased global Void Coin drops 5 -> 10
	- Increased Void Coin drop in Void Locus 15 -> 20
	- Locus guaranteed coin barrel spawn per player 7 -> 14
		- Now, it will only count up to 4 players, higher player count lobbies should get more coins from the increased monster count due to 10 man scaling.
		
- Added icons for the void coin rules. This one is for (you).
- Added a DeathRewards component to Assassins. Includes a log.
- Added a boss drop to Major Constructs.

I might merge the Void Locus changes into VoidQol in the future.

**0.0.1**
* Initial Release