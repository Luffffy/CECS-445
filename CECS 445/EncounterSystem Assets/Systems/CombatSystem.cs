using Components;
public class CombatSystem {

    // Right now unit and the character they reference have the same attack but I will probably augment unit with field that keeps
    // tracks of statuseffects(buffs/debuffs) that could make the unit attack different from the character it references
   public void LaunchAttack(Unit attacker, Unit target) {
        target.TakeDamage(attacker.Character.attack);
    }
}