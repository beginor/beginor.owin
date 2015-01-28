namespace Beginor.Owin.StaticFile {

    public interface IMimeTypeProvider {

        string DefaultMimeType { get; }

        string GetMimeType(string extension);

    }

}
