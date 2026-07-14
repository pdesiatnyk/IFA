using System.Text.Json;
using System.Text.Json.Serialization;
using IfaUdi.Parser.Models;

namespace IfaUdi.Parser.Api;

/// <summary>Maps UdiScheme to/from the same string literals the TS library's UdiScheme type uses.</summary>
public sealed class UdiSchemeJsonConverter : JsonConverter<UdiScheme>
{
    public override UdiScheme Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "PPN" => UdiScheme.Ppn,
            "HPC" => UdiScheme.Hpc,
            "MASTER_UDI_DI" => UdiScheme.MasterUdiDi,
            var value => throw new JsonException($"Unrecognized UdiScheme value '{value}'."),
        };

    public override void Write(Utf8JsonWriter writer, UdiScheme value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            UdiScheme.Ppn => "PPN",
            UdiScheme.Hpc => "HPC",
            UdiScheme.MasterUdiDi => "MASTER_UDI_DI",
            _ => throw new ArgumentOutOfRangeException(nameof(value)),
        });
}

/// <summary>Maps EnvelopeForm to/from the same string literals the TS library's EnvelopeForm type uses.</summary>
public sealed class EnvelopeFormJsonConverter : JsonConverter<EnvelopeForm>
{
    public override EnvelopeForm Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.GetString() switch
        {
            "interpretationLine" => EnvelopeForm.InterpretationLine,
            "rawIso15434" => EnvelopeForm.RawIso15434,
            "din16598" => EnvelopeForm.Din16598,
            var value => throw new JsonException($"Unrecognized EnvelopeForm value '{value}'."),
        };

    public override void Write(Utf8JsonWriter writer, EnvelopeForm value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value switch
        {
            EnvelopeForm.InterpretationLine => "interpretationLine",
            EnvelopeForm.RawIso15434 => "rawIso15434",
            EnvelopeForm.Din16598 => "din16598",
            _ => throw new ArgumentOutOfRangeException(nameof(value)),
        });
}
