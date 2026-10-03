using Mono.Cecil;
using Xunit;

namespace DamagePreview.Tests;

/// <summary>Game members AttackerReader binds to. Verified against Valheim 1.0.16 (2026-10-03).</summary>
public class AttackerApiGuardTests
{
    [Fact]
    public void Player_m_localPlayer_is_public_static()
    {
        if (!GameAssembly.IsAvailable) return;
        FieldDefinition f = GameAssembly.Field("Player", "m_localPlayer");
        Assert.True(f.IsPublic);
        Assert.True(f.IsStatic);
    }

    [Theory]
    [InlineData("GetCurrentWeapon")]
    [InlineData("GetAmmoItem")]
    [InlineData("GetAttackDrawPercentage")]
    [InlineData("GetInventory")]
    public void Humanoid_parameterless_members_are_public(string name)
    {
        if (!GameAssembly.IsAvailable) return;
        Assert.True(GameAssembly.Method("Humanoid", name).IsPublic);
    }

    [Theory]
    [InlineData("GetSkillFactor", "SkillType")]
    [InlineData("GetSEMan")]
    [InlineData("GetLevel")]
    public void Character_members_used_for_the_attacker_are_public(string name, params string[] parameters)
    {
        if (!GameAssembly.IsAvailable) return;
        Assert.True(GameAssembly.Method("Character", name, parameters).IsPublic);
    }

    [Fact]
    public void Inventory_GetAmmoItem_and_ContainsItem_match_the_game_lookup()
    {
        if (!GameAssembly.IsAvailable) return;
        Assert.True(GameAssembly.Method("Inventory", "GetAmmoItem", "String", "String").IsPublic);
        Assert.True(GameAssembly.Method("Inventory", "ContainsItem", "ItemData").IsPublic);
    }

    [Fact]
    public void SEMan_ModifyAttack_takes_skill_and_ref_hitdata()
    {
        if (!GameAssembly.IsAvailable) return;
        MethodDefinition m = GameAssembly.Method("SEMan", "ModifyAttack", "SkillType", "HitData&");
        Assert.True(m.IsPublic);
    }

    [Fact]
    public void Only_SE_Stats_overrides_ModifyAttack_so_it_stays_multiplicative()
    {
        // The adapter trusts ModifyAttack to be side-effect free. If Iron Gate adds an
        // override, re-read it before trusting the preview.
        if (!GameAssembly.IsAvailable) return;
        int overrides = 0;
        foreach (TypeDefinition t in GameAssembly.Type("StatusEffect").Module.Types)
        {
            if (t.BaseType == null) continue;
            foreach (MethodDefinition m in t.Methods)
            {
                if (m.Name == "ModifyAttack" && m.IsVirtual && !m.IsNewSlot) overrides++;
            }
        }
        Assert.Equal(1, overrides);
    }

    [Theory]
    [InlineData("m_damageMultiplier", "Single")]
    [InlineData("m_bowDraw", "Boolean")]
    public void Attack_fields_are_public(string name, string type)
    {
        if (!GameAssembly.IsAvailable) return;
        FieldDefinition f = GameAssembly.Field("Attack", name);
        Assert.True(f.IsPublic);
        Assert.Equal(type, f.FieldType.Name);
    }

    [Fact]
    public void ItemData_GetDamage_and_HaveSecondaryAttack_are_public()
    {
        if (!GameAssembly.IsAvailable) return;
        TypeDefinition itemData = GameAssembly.NestedType("ItemDrop", "ItemData");
        Assert.Contains(itemData.Methods, m => m.Name == "GetDamage" && m.Parameters.Count == 0 && m.IsPublic);
        Assert.Contains(itemData.Methods, m => m.Name == "HaveSecondaryAttack" && m.IsPublic);
        Assert.Contains(itemData.Fields, f => f.Name == "m_shared" && f.IsPublic);
    }

    [Theory]
    [InlineData("m_attack", "Attack")]
    [InlineData("m_secondaryAttack", "Attack")]
    [InlineData("m_skillType", "SkillType")]
    [InlineData("m_backstabBonus", "Single")]
    [InlineData("m_ammoType", "String")]
    public void SharedData_fields_are_public(string name, string type)
    {
        if (!GameAssembly.IsAvailable) return;
        TypeDefinition itemData = GameAssembly.NestedType("ItemDrop", "ItemData");
        TypeDefinition shared = System.Linq.Enumerable.First(itemData.NestedTypes, t => t.Name == "SharedData");
        FieldDefinition f = System.Linq.Enumerable.First(shared.Fields, x => x.Name == name);
        Assert.True(f.IsPublic);
        Assert.Equal(type, f.FieldType.Name);
    }

    [Fact]
    public void HitData_DamageTypes_has_the_twelve_channels_and_HitData_m_damage_is_public()
    {
        if (!GameAssembly.IsAvailable) return;
        TypeDefinition dt = GameAssembly.NestedType("HitData", "DamageTypes");
        string[] expected = { "m_damage", "m_blunt", "m_slash", "m_pierce", "m_chop", "m_pickaxe",
            "m_fire", "m_frost", "m_lightning", "m_poison", "m_spirit", "m_nonPlayer" };
        foreach (string name in expected)
        {
            Assert.Contains(dt.Fields, f => f.Name == name && f.IsPublic && f.FieldType.Name == "Single");
        }
        Assert.Equal(expected.Length, System.Linq.Enumerable.Count(dt.Fields, f => !f.IsStatic));
        Assert.True(GameAssembly.Field("HitData", "m_damage").IsPublic);
    }

    [Fact]
    public void Formulas_we_reimplement_still_exist_with_the_same_signature()
    {
        if (!GameAssembly.IsAvailable) return;
        Assert.NotNull(GameAssembly.Method("Skills", "GetRandomSkillFactor", "SkillType"));
        Assert.NotNull(GameAssembly.Method("Attack", "GetLevelDamageFactor"));
        TypeDefinition dt = GameAssembly.NestedType("HitData", "DamageTypes");
        Assert.Contains(dt.Methods, m => m.Name == "ApplyArmor" && m.Parameters.Count == 2 && m.IsStatic);
        Assert.Contains(dt.Methods, m => m.Name == "ApplyArmor" && m.Parameters.Count == 1);
        Assert.NotNull(GameAssembly.Method("HitData", "ApplyResistance", "DamageModifiers", "DamageModifier&"));
    }
}
