
using UnityEngine;
using System.Collections.Generic;
public enum PhysicEffectsEnum
{
    Burn,
    Wet,
    Cool,
    Frosen,
    Electric,
    petrifaction
}
public class EffectController : MonoBehaviour
{
    List<PhysicEffects> pes = new List<PhysicEffects>();
    void Start()
    {
        
    }

    void Update()
    {
        foreach (PhysicEffects pe in pes)
        {
            pe.ApplyEffect();
        }
    }

    public void AddEffect(PhysicEffects _pe)
    {
        foreach(PhysicEffects pe in pes)
        {
            if(_pe.EffectReact(pe.pee))
            {
                pes.Remove(pe);
            }
            else
            {
                pes.Add(_pe);
            }
        }
    }
}

public abstract class PhysicEffects
{
    protected float effectTime;
    protected GameObject effectApplier;
    public readonly PhysicEffectsEnum pee;
    public abstract void ApplyEffect();
    public abstract bool EffectReact(PhysicEffectsEnum compare);
    public PhysicEffects(float _effectTime, PhysicEffectsEnum _pee, GameObject _effectApplier)
    {
        effectTime = _effectTime;
        pee = _pee;
        effectApplier = _effectApplier;
    }
}

public class BurnEffect : PhysicEffects
{
    private AttributesController ac;
    private float cd;
    public override void ApplyEffect()
    {
        if(cd >= 1)
        {
            ac.OnHealthChanging(null, 2, 0, Vector2.zero);
            cd = 0;
        }
        else
        {
            cd += Time.deltaTime;
        }
        
    }
    public override bool EffectReact(PhysicEffectsEnum compare)
    {
        switch(compare)
        {
            case PhysicEffectsEnum.Wet:
                return true;
            case PhysicEffectsEnum.Cool:
                return true;
            default: 
                return false;
        }
    }
    public BurnEffect(float _effectTime, PhysicEffectsEnum _pee, GameObject _effectApplier, StatusController _sc, AttributesController _ac) : base(_effectTime, _pee, _effectApplier)
    {
        ac = _ac;
    }
}

