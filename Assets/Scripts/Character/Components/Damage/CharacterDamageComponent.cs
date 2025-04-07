public class CharacterDamageComponent : IDamageComponent
{
    private Character character;

    public CharacterDamageComponent(Character character)
    {
        this.character = character;
    }

    public float Damage => 10;

    public void MakeDamage(Character characterTarget)
    {
        if (characterTarget.LiveComponent != null)
            characterTarget.LiveComponent.SetDamage(Damage);
    }
}
