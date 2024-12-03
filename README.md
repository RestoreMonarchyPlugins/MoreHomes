# MoreHomes
Allow players to claim and teleport to multiple beds. 

## Features
* Allow players to claim multiple beds
* Cooldown and delay system for home teleportation
* Cancel teleportation if the player moves
* Integrates with Telepoertation plugin for Combat and Raid mode checks
* VIP permission system for cooldown, delay, and max homes
* As of version **v1.9.0,** `MoreHomesData.json` is stored in the map level data folder instead of the plugin directory

## Commands
### Player
* **/home** – Teleport to the first home in the list
* **/home \<name\>** – Teleport to the home with the given name
* **/homes** – Lists all your homes
* **/destroyhome \<name\>** – Removes the home with the given name
* **/renamehome \<name\> \<new_name\>** – Renames the home with the given name
### Admin/Console
* **/restorehomes** - Collects all the homes from the map and restores them to `MoreHomesData.json`

## Player Permissions
```xml
<Permission Cooldown="0">home</Permission>
<Permission Cooldown="0">homes</Permission>
<Permission Cooldown="0">destroyhome</Permission>
<Permission Cooldown="0">renamehome</Permission>
```

## Configuration
```xml
<?xml version="1.0" encoding="utf-8"?>
<MoreHomesConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <MessageColor>yellow</MessageColor>
  <MessageIconUrl>https://i.imgur.com/9TF5aB1.png</MessageIconUrl>
  <DefaultHomeCooldown>20</DefaultHomeCooldown>
  <DefaultHomeDelay>10</DefaultHomeDelay>
  <DefaultMaxHomes>2</DefaultMaxHomes>
  <TeleportHeight>0.5</TeleportHeight>
  <CancelOnMove>true</CancelOnMove>
  <MoveMaxDistance>0.5</MoveMaxDistance>
  <BlockUnderground>false</BlockUnderground>
  <VIPCooldowns>
    <VIPPermission PermissionTag="morehomes.vip" Value="10" />
    <VIPPermission PermissionTag="morehomes.star" Value="5" />
  </VIPCooldowns>
  <VIPDelays>
    <VIPPermission PermissionTag="morehomes.vip" Value="5" />
    <VIPPermission PermissionTag="morehomes.star" Value="3" />
  </VIPDelays>
  <VIPMaxHomes>
    <VIPPermission PermissionTag="morehomes.vip" Value="3" />
    <VIPPermission PermissionTag="morehomes.star" Value="4" />
  </VIPMaxHomes>
</MoreHomesConfiguration>
```

## Translations
```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="HomeCooldown" Value="Please wait [[b]]{0}[[/b]] seconds before using home again" />
  <Translation Id="HomeDelayWarn" Value="You will be teleported to your home [[b]]{0}[[/b]] in seconds" />
  <Translation Id="MaxHomesWarn" Value="You have reached the maximum number of beds" />
  <Translation Id="BedDestroyed" Value="Home unavailable: bed destroyed or unclaimed. Teleportation canceled" />
  <Translation Id="WhileDriving" Value="You can't teleport while driving" />
  <Translation Id="NoHome" Value="No matching home found for teleportation" />
  <Translation Id="HomeSuccess" Value="You were teleported to [[b]]{0}[[/b]] home" />
  <Translation Id="HomeList" Value="Your homes [[b]][{0}/{1}][[/b]]: " />
  <Translation Id="NoHomes" Value="You don't have any claimed beds" />
  <Translation Id="DestroyHomeFormat" Value="Usage: /destroyhome [[name]]" />
  <Translation Id="HomeNotFound" Value="No home found with the name [[b]]{0}[[/b]]" />
  <Translation Id="DestroyHomeSuccess" Value="Home [[b]]{0}[[/b]] has been removed" />
  <Translation Id="RenameHomeFormat" Value="Usage: /renamehome [[current name]] [[new name]]" />
  <Translation Id="HomeAlreadyExists" Value="You already have home with the name [[b]]{0}[[/b]]" />
  <Translation Id="RenameHomeSuccess" Value="Home renamed from [[b]]{0}[[/b]] to [[b]]{1}[[/b]]" />
  <Translation Id="WhileRaid" Value="You can't teleport while in raiding mode" />
  <Translation Id="WhileCombat" Value="You can't teleport while in combat mode" />
  <Translation Id="RestoreHomesSuccess" Value="[[b]]{0}[[/b]] homes have been restored" />
  <Translation Id="RemoveHome" Value="Your [[b]]{0}[[/b]] home has been removed" />
  <Translation Id="HomeClaimed" Value="New home claimed with the name [[b]]{0}[[/b]]" />
  <Translation Id="HomeTeleportationFailed" Value="Failed to teleport to [[b]]{0}[[/b]] home" />
  <Translation Id="HomeDestroyed" Value="Your [[b]]{0}[[/b]] home was destroyed" />
  <Translation Id="HomeCanceledYouMoved" Value="Home teleportation canceled because you moved" />
  <Translation Id="CantTeleportToBedUnderground" Value="You can't teleport to [[b]]{0}[[/b]] home, because it's underground." />
</Translations>
```
