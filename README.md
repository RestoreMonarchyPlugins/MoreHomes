[![Version](https://img.shields.io/github/release/RestoreMonarchyPlugins/MoreHomes.svg)](https://github.com/RestoreMonarchyPlugins/MoreHomes/releases) ![Discord](https://discordapp.com/api/guilds/520355060312440853/widget.png)
# MoreHomes
* Allows players to have multiple beds
* You can set the delay of teleportation
* Manage max amount of homes by creating permissions
* Restore all beds to the database with a command
* Checks if bed exists before teleporting

## Commands

**/home** *<Name>* – Teleports player to their bed
``` 
<Permission Cooldown="0">home</Permission>
```
**/homes** – Displays a list of player's claimed beds
``` 
<Permission Cooldown="0">homes</Permission>
```
**/destroyhome** *<Name>* – Destroys the bed and removes it from you home list
```
<Permission Cooldown="0">destroyhome</Permission>
```
**/renamehome** *<Name>* *<NewName>* – Changes home's name to a new one
``` 
<Permission Cooldown="0">renamehome</Permission>
```
**/restorehomes** - Restores all beds that are claimed and not in database
``` 
<Permission Cooldown="0">restorehomes</Permission>
```

## Configuration
```xml
<?xml version="1.0" encoding="utf-8"?>
<MoreHomesConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <MessageColor>#f79122</MessageColor>
  <DefaultHomeCooldown>20</DefaultHomeCooldown>
  <DefaultHomeDelay>10</DefaultHomeDelay>
  <DefaultMaxHomes>2</DefaultMaxHomes>
  <TeleportHeight>0.5</TeleportHeight>
  <CancelOnMove>true</CancelOnMove>
  <MoveMaxDistance>0.5</MoveMaxDistance>
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
  <Translation Id="HomeCooldown" Value="You have to wait {0} to use this command again" />
  <Translation Id="HomeDelayWarn" Value="You will be teleported to your home in {0} seconds" />
  <Translation Id="MaxHomesWarn" Value="You cannot have more homes" />
  <Translation Id="BedDestroyed" Value="Your home got destroyed or unclaimed! Teleportation canceled" />
  <Translation Id="WhileDriving" Value="You cannot teleport while driving" />
  <Translation Id="NoHome" Value="You don't have any home to teleport or name doesn't match any" />
  <Translation Id="HomeSuccess" Value="Successfully teleported You to your {0} home!" />
  <Translation Id="HomeList" Value="Your homes [{0}/{1}]: " />
  <Translation Id="NoHomes" Value="You don't have any home" />
  <Translation Id="DestroyHomeFormat" Value="Format: /destroyhome &lt;name&gt;" />
  <Translation Id="HomeNotFound" Value="No home match {0} name" />
  <Translation Id="DestroyHomeSuccess" Value="Successfully destroyed your home {0}!" />
  <Translation Id="RenameHomeFormat" Value="Format: /renamehome &lt;name&gt; &lt;rename&gt;" />
  <Translation Id="HomeAlreadyExists" Value="You already have a home named {0}" />
  <Translation Id="RenameHomeSuccess" Value="Successfully renamed home {0} to {1}!" />
  <Translation Id="WhileRaid" Value="You can't teleport while in raiding" />
  <Translation Id="WhileCombat" Value="You can't teleport while in combat" />
  <Translation Id="RestoreHomesSuccess" Value="Successfully restored {0} homes!" />
  <Translation Id="RemoveHome" Value="Your {0} home got removed!" />
  <Translation Id="RenameHomeSuccess" Value="Successfully renamed home {0} to {1}!" />
  <Translation Id="HomeClaimed" Value="Your new claimed home name is {0}" />
  <Translation Id="HomeTeleportationFailed" Value="Failed to teleport you to {0} home" />
  <Translation Id="HomeDestroyed" Value="Your home {0} got destroyed or you salvaged it!" />
</Translations>
```
