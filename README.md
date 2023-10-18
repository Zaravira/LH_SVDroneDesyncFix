# Description
This mod fixes the drone bay desync that happens when you have multiple drone bays stacked.
Whenever a drone strays too far from the ship or can't find a target in its general vincinity, it would disable that bay.
This causes some drone bays to be active and others to be disabled, and because they're stacked you have no way of seeing this happen, and no way to reactivate those bays.
This happens a lot more often than you realise and can be very problematic if you're relying on repair drone bays.

This mod changes the drone behaviour so all mining drones, and attack drones with auto targetting, stay active until none of them can find a target.
Repair drones with auto targetting stay active and return to sit on your own ship. 
This mod takes over at the point where the original code fails.

# Installing
Simply grab the .dll from the release and stick it into steam\steamapps\common\Star Valor\BepInEx\plugins folder. 
You'll have to install BepInEx first if you haven't already though.

# Credit
This mod was made using MartinC's defensive attack drone mod, which partially fixed the desync issue with attack drones by accident. 
Although almost none of the code of the original mod remains, it gave me a good starting point by showing me how to change drone behaviour.
