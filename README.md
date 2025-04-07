# MoreHomes

A plugin for Unturned that allows players to claim and teleport to multiple beds.

## Features
- Claim multiple beds as homes
- Teleport to your homes with simple commands
- Configurable cooldown and delay system
- Teleportation cancels if player moves
- Works with Teleportation plugin to check combat and raid status
- VIP system for special cooldowns, delays, and home limits

## Commands

### Player Commands
- `/home` - Teleport to your first home
- `/home <name>` - Teleport to a specific home
- `/homes` - List all your homes
- `/destroyhome <name>` - Delete a home
- `/renamehome <name> <new_name>` - Rename a home

### Admin Commands
- `/restorehomes` - Rebuild homes database from all beds on the map (console only)

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

### Configuration Options

- `MessageColor` - Color of plugin messages to players
- `MessageIconUrl` - URL of the icon shown in messages
- `DefaultHomeCooldown` - Seconds players must wait between using home teleport
- `DefaultHomeDelay` - Seconds players must wait before teleportation happens
- `DefaultMaxHomes` - Maximum number of homes a player can have
- `TeleportHeight` - Height offset when teleporting to a bed
- `CancelOnMove` - Whether teleportation should cancel if player moves
- `MoveMaxDistance` - How far a player can move before teleport cancels
- `BlockUnderground` - Prevent teleporting to beds located underground

## VIP System

The VIP system uses permission tags to give players special benefits:

- `VIPCooldowns` - Custom cooldown times for VIP players
- `VIPDelays` - Custom delay times for VIP players
- `VIPMaxHomes` - Custom maximum home limits for VIP players

Each VIP setting contains:
- `PermissionTag` - The permission node that players need to have
- `Value` - The numeric value for that permission

For example:
- `<VIPPermission PermissionTag="morehomes.vip" Value="3" />` in VIPMaxHomes means players with "morehomes.vip" permission can have 3 homes
- `<VIPPermission PermissionTag="morehomes.star" Value="5" />` in VIPCooldowns means players with "morehomes.star" permission have a 5 second cooldown

The plugin always uses the best value for the player:
- For cooldowns and delays: the lowest value from applicable permissions
- For max homes: the highest value from applicable permissions

## Player Permissions

```xml
<Permission Cooldown="0">home</Permission>
<Permission Cooldown="0">homes</Permission>
<Permission Cooldown="0">destroyhome</Permission>
<Permission Cooldown="0">renamehome</Permission>
```