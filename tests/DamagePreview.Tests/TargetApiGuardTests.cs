using Mono.Cecil;
using Xunit;

namespace DamagePreview.Tests;

/// <summary>Game members TargetReader binds to. Verified against Valheim 1.0.16 (2026-10-03).</summary>
public class TargetApiGuardTests
{
    [Theory]
    [InlineData("IsStaggering")]
    [InlineData("GetHealth")]
    [InlineData("GetMaxHealth")]
    [InlineData("IsPlayer")]
    public void Character_parameterless_members_are_public(string name)
    {
        if (!GameAssembly.IsAvailable) return;
        Assert.True(GameAssembly.Method("Character", name).IsPublic);
    }

    [Fact]
    public void Character_GetDamageModifiers_takes_an_optional_weakspot()
    {
        if (!GameAssembly.IsAvailable) return;
        MethodDefinition m = GameAssembly.Method("Character", "GetDamageModifiers", "WeakSpot");
        Assert.True(m.IsPublic);
        Assert.True(m.Parameters[0].IsOptional);
    }

    [Fact]
    public void Character_m_baseAI_and_m_backstabTime_exist()
    {
        if (!GameAssembly.IsAvailable) return;
        Assert.Equal("BaseAI", GameAssembly.Field("Character", "m_baseAI").FieldType.Name);
        Assert.Equal("Single", GameAssembly.Field("Character", "m_backstabTime").FieldType.Name);
    }

    [Fact]
    public void Backstab_cooldown_is_still_300_seconds()
    {
        // RPC_Damage: Time.time - m_backstabTime > 300f. Pinned by scanning the IL for the constant.
        if (!GameAssembly.IsAvailable) return;
        MethodDefinition m = GameAssembly.Method("Character", "RPC_Damage", "Int64", "HitData");
        Assert.Contains(m.Body.Instructions, i => i.Operand is float f && f == 300f);
    }

    [Fact]
    public void BaseAI_IsAlerted_is_public()
    {
        if (!GameAssembly.IsAvailable) return;
        Assert.True(GameAssembly.Method("BaseAI", "IsAlerted").IsPublic);
    }

    [Theory]
    [InlineData("m_worldLevel", "Int32", true)]
    [InlineData("m_playerDamageRate", "Single", true)]
    [InlineData("m_worldLevelEnemyBaseAC", "Int32", false)]
    public void Game_fields_are_public(string name, string type, bool isStatic)
    {
        if (!GameAssembly.IsAvailable) return;
        FieldDefinition f = GameAssembly.Field("Game", name);
        Assert.True(f.IsPublic);
        Assert.Equal(type, f.FieldType.Name);
        Assert.Equal(isStatic, f.IsStatic);
    }

    [Fact]
    public void Game_GetDifficultyDamageScaleEnemy_takes_a_position()
    {
        if (!GameAssembly.IsAvailable) return;
        Assert.True(GameAssembly.Method("Game", "GetDifficultyDamageScaleEnemy", "Vector3").IsPublic);
        // Game.instance is an auto-property with a public static getter.
        PropertyDefinition instance = System.Linq.Enumerable.First(GameAssembly.Type("Game").Properties, p => p.Name == "instance");
        Assert.True(instance.GetMethod.IsPublic);
        Assert.True(instance.GetMethod.IsStatic);
    }

    [Fact]
    public void HitData_DamageModifiers_has_the_eleven_resisted_channels()
    {
        if (!GameAssembly.IsAvailable) return;
        TypeDefinition dm = GameAssembly.NestedType("HitData", "DamageModifiers");
        string[] expected = { "m_blunt", "m_slash", "m_pierce", "m_chop", "m_pickaxe",
            "m_fire", "m_frost", "m_lightning", "m_poison", "m_spirit", "m_nonPlayer" };
        foreach (string name in expected)
        {
            Assert.Contains(dm.Fields, f => f.Name == name && f.IsPublic && f.FieldType.Name == "DamageModifier");
        }
    }

    [Fact]
    public void HitData_DamageModifier_enum_order_matches_core()
    {
        if (!GameAssembly.IsAvailable) return;
        TypeDefinition e = GameAssembly.NestedType("HitData", "DamageModifier");
        string[] expected = { "Normal", "Resistant", "Weak", "Immune", "Ignore", "VeryResistant", "VeryWeak", "SlightlyResistant", "SlightlyWeak" };
        for (int i = 0; i < expected.Length; i++)
        {
            FieldDefinition f = System.Linq.Enumerable.First(e.Fields, x => x.Name == expected[i]);
            Assert.Equal(i, (int)f.Constant);
        }
    }

    [Fact]
    public void Character_m_weakSpots_is_a_public_WeakSpot_array()
    {
        if (!GameAssembly.IsAvailable) return;
        FieldDefinition f = GameAssembly.Field("Character", "m_weakSpots");
        Assert.True(f.IsPublic);
        Assert.Equal("WeakSpot[]", f.FieldType.Name);
    }

    [Fact]
    public void WeakSpot_m_damageModifiers_is_public()
    {
        if (!GameAssembly.IsAvailable) return;
        FieldDefinition f = GameAssembly.Field("WeakSpot", "m_damageModifiers");
        Assert.True(f.IsPublic);
        Assert.Equal("DamageModifiers", f.FieldType.Name);
    }
}
