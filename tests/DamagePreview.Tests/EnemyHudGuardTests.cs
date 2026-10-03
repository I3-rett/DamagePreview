using Mono.Cecil;
using Xunit;

namespace DamagePreview.Tests;

/// <summary>The hud members the Harmony patch and the ghost renderer bind to.
/// Verified against Valheim 1.0.16 (2026-10-03). Private members are reached through
/// the publicized reference assembly, so visibility is not asserted, existence and shape are.</summary>
public class EnemyHudGuardTests
{
    [Fact]
    public void EnemyHud_instance_getter_is_public_static()
    {
        if (!GameAssembly.IsAvailable) return;
        PropertyDefinition p = System.Linq.Enumerable.First(GameAssembly.Type("EnemyHud").Properties, x => x.Name == "instance");
        Assert.True(p.GetMethod.IsPublic);
        Assert.True(p.GetMethod.IsStatic);
    }

    [Fact]
    public void EnemyHud_UpdateHuds_takes_player_sadle_dt()
    {
        if (!GameAssembly.IsAvailable) return;
        MethodDefinition m = GameAssembly.Method("EnemyHud", "UpdateHuds", "Player", "Sadle", "Single");
        Assert.False(m.IsStatic);
    }

    [Fact]
    public void EnemyHud_m_huds_is_a_dictionary_of_character_to_HudData()
    {
        if (!GameAssembly.IsAvailable) return;
        FieldDefinition f = GameAssembly.Field("EnemyHud", "m_huds");
        Assert.StartsWith("Dictionary`2", f.FieldType.Name);
        var generic = (GenericInstanceType)f.FieldType;
        Assert.Equal("Character", generic.GenericArguments[0].Name);
        Assert.Equal("HudData", generic.GenericArguments[1].Name);
    }

    [Theory]
    [InlineData("m_character", "Character")]
    [InlineData("m_gui", "GameObject")]
    [InlineData("m_healthFast", "GuiBar")]
    [InlineData("m_healthSlow", "GuiBar")]
    [InlineData("m_isMount", "Boolean")]
    public void HudData_fields_used_by_the_renderer_exist(string name, string type)
    {
        if (!GameAssembly.IsAvailable) return;
        TypeDefinition hudData = GameAssembly.NestedType("EnemyHud", "HudData");
        FieldDefinition? f = System.Linq.Enumerable.FirstOrDefault(hudData.Fields, x => x.Name == name);
        Assert.NotNull(f);
        Assert.Equal(type, f!.FieldType.Name);
    }

    [Theory]
    [InlineData("m_bar", "RectTransform")]
    [InlineData("m_width", "Single")]
    public void GuiBar_fields_used_by_the_renderer_exist(string name, string type)
    {
        if (!GameAssembly.IsAvailable) return;
        FieldDefinition f = GameAssembly.Field("GuiBar", name);
        Assert.Equal(type, f.FieldType.Name);
    }
}
