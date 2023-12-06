

namespace StructuredLogging
{
  public static partial class ArtikelEndpoints
  {
    public record Info(string Infotext = "eine Info");

    [LoggerMessage(Message = "Bearbeitung Artikel Nr. {artikelnummer}", EventId = 123)]
    public static partial void LogArtikelV3Precompiled(this ILogger logger, int artikelnummer, [LogProperties] Artikel artikel, [LogProperties] Info info, LogLevel logLevel = LogLevel.Warning);

    public static void LogArtikelV3Precompiled(this ILogger logger, Artikel artikel, LogLevel logLevel = LogLevel.Warning) =>
      LogArtikelV3Precompiled(logger, artikel.Nummer, artikel, new Info(), logLevel);

    [LoggerMessage(Message = "Bearbeitung Artikel {artikel}", EventId = 123)]
    public static partial void LogArtikelV4Precompiled(this ILogger logger, Artikel artikel, LogLevel logLevel = LogLevel.Warning);

    [LoggerMessage("Logging a key/value-list", Level = LogLevel.Error)]
    public static partial void LogKeyValueList(this ILogger logger, Dictionary<string, object> myData);

    [LoggerMessage("Logging a list", Level = LogLevel.Critical)]
    public static partial void LogList(this ILogger logger, IEnumerable<string> myList);

    [LoggerMessage("Logging an exception", Level = LogLevel.Critical)]
    public static partial void LogException(this ILogger logger, Exception exception, [LogProperties] Artikel artikel);

    public static WebApplication MapArtikelEndpoints(this WebApplication app)
    {
      var group = app.MapGroup("artikel")
        .WithOpenApi();

      group.MapGet("", GetArtikel);
      group.MapGet("v2", GetArtikelV2);
      group.MapGet("v3", GetArtikelV3);
      group.MapGet("v4", GetArtikelV4);
      group.MapGet("errorlist", GetWithErrorlist);
      group.MapGet("keyvaluelist", GetWithKeyValuelist);
      group.MapGet("exception", GetWithException);

      return app;
    }

    private static IResult GetWithException(ILoggerFactory loggerFactory)
    {
      var logger = loggerFactory.CreateLogger(nameof(ArtikelEndpoints));
      var artikel = new Artikel { Bezeichnung = "Kaputte Uhr", Nummer = 77777, Preis = 13.13, Herstellerhinweise = new() { "sehr schöne Standuhr", "zeigt 2x täglich die richtige Zeit an" } };

      try
      {
        throw new ApplicationException("damit war zu rechnen...", new InvalidOperationException("so geht das nicht..."));
      }
      catch (Exception ex)
      {
        logger.LogException(ex, artikel);
      }

      return Results.Problem("das ist ein Problem");
    }

    private static string GetWithKeyValuelist(ILoggerFactory loggerFactory)
    {
      var logger = loggerFactory.CreateLogger(nameof(ArtikelEndpoints));
      var artikel = new Artikel { Bezeichnung = "Radio", Nummer = 44838, Preis = 45.6, Herstellerhinweise = new() { "nur UKW", "funktioniert nur bei schönem Wetter" } };
      Dictionary<string, object> data = new Dictionary<string, object> { ["info"] = "nur zur Information", ["date"] = DateTime.Now, ["artikel"] = artikel };

      logger.LogKeyValueList(data);
      return "Hallo";
    }
    private static string GetWithErrorlist(ILoggerFactory loggerFactory)
    {
      var logger = loggerFactory.CreateLogger(nameof(ArtikelEndpoints));
      logger.LogList(["Artikel nicht vorhanden", "Ausverkauft"]);
      return "Hallo";
    }

    private static Artikel GetArtikel(ILoggerFactory loggerFactory)
    {
      var logger = loggerFactory.CreateLogger(nameof(ArtikelEndpoints));

      var artikel = new Artikel { Bezeichnung = "Radio", Nummer = 44838, Preis = 45.6, Herstellerhinweise = new() { "nur UKW", "funktioniert nur bei schönem Wetter" } };

      logger.LogInformation("Ware bestellt: {bezeichnung}, Artikelnummer: {artikelnummer}, Preis: {preis}, Herstellerhinweise: {hinweise}", artikel.Bezeichnung, artikel.Nummer, artikel.Preis, artikel.Herstellerhinweise);

      return artikel;
    }

    private static Artikel GetArtikelV2(ILoggerFactory loggerFactory)
    {
      var logger = loggerFactory.CreateLogger(nameof(ArtikelEndpoints));

      var artikel = new Artikel { Bezeichnung = "Radio", Nummer = 44838, Preis = 45.6, Herstellerhinweise = new() { "nur UKW", "funktioniert nur bei schönem Wetter" } };

      logger.LogInformation("Ware bestellt: {artikel}", artikel);

      return artikel;
    }

    private static Artikel GetArtikelV3(ILoggerFactory loggerFactory)
    {
      var logger = loggerFactory.CreateLogger(nameof(ArtikelEndpoints));

      var artikel = new Artikel { Bezeichnung = "Radio", Nummer = 44838, Preis = 45.6, Herstellerhinweise = new() { "nur UKW", "funktioniert nur bei schönem Wetter" } };

      logger.LogArtikelV3Precompiled(artikel);

      return artikel;
    }

    private static Artikel GetArtikelV4(ILoggerFactory loggerFactory)
    {
      var logger = loggerFactory.CreateLogger(nameof(ArtikelEndpoints));

      var artikel = new Artikel { Bezeichnung = "Radio", Nummer = 44838, Preis = 45.6, Herstellerhinweise = new() { "nur UKW", "funktioniert nur bei schönem Wetter" } };

      logger.LogArtikelV4Precompiled(artikel);

      return artikel;
    }


  }
}
