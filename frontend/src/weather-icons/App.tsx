import "./App.css";
import { iconCategories } from "../icons-data";

//WW

type WeatherData = {
  snow?: number;
  rain?: number;
  fog?: number;
  cloud?: string;
};

function getWeatherIconClass(weather?: WeatherData): string {
  if (!weather) return "wi-day-sunny";
  if (weather.snow) return "wi-snow";
  if (weather.rain) return "wi-rain";
  if (weather.fog) return "wi-fog";
  switch (weather.cloud) {
    case "OVC":
      return "wi-cloudy";
    case "BKN":
      return "wi-cloudy";
    case "SCT":
      return "wi-day-cloudy";
    case "FEW":
      return "wi-day-cloudy";
    default:
      return "wi-day-sunny";
  }
}

// Exempeldata för att visa funktionen i UI
const exampleConditions: { label: string; data: WeatherData }[] = [
  { label: "Soligt", data: {} },
  { label: "Regn (2 mm)", data: { rain: 2 } },
  { label: "Snö (5 cm)", data: { snow: 5 } },
  { label: "Dimma", data: { fog: 1 } },
  { label: "Mulet (OVC)", data: { cloud: "OVC" } },
  { label: "Brutet moln (BKN)", data: { cloud: "BKN" } },
  { label: "Spridd (SCT)", data: { cloud: "SCT" } },
  { label: "Lite moln (FEW)", data: { cloud: "FEW" } },
];

function App() {
  return (
    <div className="gallery">
      <header className="gallery-header">
        <h1>Weather Icons</h1>
      </header>

      {/* --- getWeatherIconClass() --- */}
      <section className="category">
        <h2 className="category-title">Exempel — getWeatherIconClass()</h2>
        <p className="category-desc">
          Funktionen tar ett väder-objekt och returnerar rätt ikonklass. Använd
          sedan klassen på ett <code>&lt;i&gt;</code>-element.
        </p>

        <div className="icon-grid">
          {exampleConditions.map(({ label, data }) => {
            const iconClass = getWeatherIconClass(data);
            return (
              <div key={label} className="icon-card">
                <i className={`wi ${iconClass}`}></i>
                <span className="icon-name">{label}</span>
                <code className="icon-code">{iconClass}</code>
              </div>
            );
          })}
        </div>
      </section>

      {/* --- Alla ikoner per kategori --- */}
      {iconCategories.map((cat) => (
        <section key={cat.name} className="category">
          <h2>{cat.name}</h2>
          <div className="icon-grid">
            {cat.icons.map((icon) => (
              <div key={icon} className="icon-card">
                <i className={`wi ${icon}`}></i>
                <span className="icon-name">{icon.replace("wi-", "")}</span>
              </div>
            ))}
          </div>
        </section>
      ))}
    </div>
  );
}

export default App;
