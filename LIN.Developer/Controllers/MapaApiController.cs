using LIN.Developer.ApiModels;
using Newtonsoft.Json;

namespace LIN.Developer.Controllers;


[Route("api/mapas")]
public class MapaApiController : Controller
{


    [HttpGet("searh")]
    public async Task<List<PlaceDataModel>> Create([FromQuery] string param, [FromQuery] int limit)
    {

        // Url del servicio 
        string url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{param}.json?access_token=pk.eyJ1IjoiYWxleDIyMDkiLCJhIjoiY2xmeGVqZ2FwMHFsajNjczZlMnY0ZDFucSJ9.NGqSheAZ0xhWtEsudyEhQA&limit={limit}";


        // Ejecucion
        try
        {

            // Envia la solicitud
            var response = await new HttpClient().GetAsync(url);

            // Lee la respuesta del servidor
            string responseContent = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<MapboxGeocodingResponse>(responseContent) ?? new();



            var places = new List<PlaceDataModel>();


            foreach (var feature in obj.Features)
            {
                try
                {
                    var place = new PlaceDataModel()
                    {
                        Text = feature.Text,
                        Nombre = feature.PlaceName,
                        Longitud = feature.Center[0].ToString().Replace(',', '.'),
                        Latitud = feature.Center[1].ToString().Replace(',', '.')
                    };
                    places.Add(place);
                }
                catch
                {

                }

            }

            return places ?? new();

        }
        catch
        {
        }

        return new();


    }



    [HttpGet("searh/around")]
    public async Task<List<PlaceDataModel>> Around([FromQuery] string param, [FromQuery] int limit, [FromQuery] string latitud, [FromQuery] string longitud)
    {

        longitud = longitud.Replace(".", ",");
        latitud = latitud.Replace(".", ",");
        var coor = BoundingBox.CreateBoundingBox(double.Parse(longitud), double.Parse(latitud), 10);

        // Url del servicio 
        string url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{param}.json?access_token=pk.eyJ1IjoiYWxleDIyMDkiLCJhIjoiY2xmeGVqZ2FwMHFsajNjczZlMnY0ZDFucSJ9.NGqSheAZ0xhWtEsudyEhQA&bbox={coor.MinX.ToString().Replace(',', '.')},{coor.MinY.ToString().Replace(',', '.')},{coor.MaxX.ToString().Replace(',', '.')},{coor.MaxY.ToString().Replace(',', '.')}&limit={limit}";


        // Ejecucion
        try
        {

            // Envia la solicitud
            var response = await new HttpClient().GetAsync(url);

            // Lee la respuesta del servidor
            string responseContent = await response.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<MapboxGeocodingResponse>(responseContent) ?? new();



            var places = new List<PlaceDataModel>();


            foreach (var feature in obj.Features)
            {
                try
                {
                    var place = new PlaceDataModel()
                    {
                        Text = feature.Text,
                        Nombre = feature.PlaceName,
                        Longitud = feature.Center[0].ToString().Replace(',', '.'),
                        Latitud = feature.Center[1].ToString().Replace(',', '.')
                    };
                    places.Add(place);
                }
                catch
                {

                }

            }

            return places ?? new();

        }
        catch
        {
        }

        return new();


    }



}