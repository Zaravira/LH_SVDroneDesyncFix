# Description
This mod fixes the drone bay desync that happens when you have multiple drone bays stacked.
Whenever a drone strays too far from the ship or can't find a target in its general vincinity, it would disable that bay.
This causes some drone bays to be active and others to be disabled, and because they're stacked you have no way of seeing this happen, and no way to reactivate those bays.
This happens a lot more often than you realise and can be very problematic if you're relying on repair drone bays.

This mod changes the drone behaviour so all mining drones, and attack drones with auto targetting, stay active until none of them can find a target.
Repair drones with auto targetting stay active and return to sit on your own ship. All drones now share an identical target zone to remain synchronised.

This means getting used to their new behaviour, they pick targets based on proximity to your own ship. 
This means:
1) Early game, your attack drones are fixated with covering your behind, instead of going off on their own.
2) You can 'aim' automatic drones by positioning your ship near priority targets.
3) Repair drones looking for a target will prioritise your ship for repairs.

# Limitations
There's a very tiny little bug hardly worth mentioning. 
When a drone is out of range of your ship, it will not listen when you recall it by disabling the bays yourself.
The only time this is really apparent is when you're deliberately flying away from your drones, and then disable the bays while some are in range and some are out of range.
This will cause a desync, so leave them be until you can stop to reorganise!

# Installing
Simply grab the .dll from the release and stick it into steam\steamapps\common\Star Valor\BepInEx\plugins folder. 
You'll have to install BepInEx first if you haven't already though.

# Credit
This mod was made using MartinC's defensive attack drone mod, which partially fixed the desync issue with attack drones by accident. 
Although almost none of the code of the original mod remains, it gave me a good starting point by showing me how to change drone behaviour.
