import { showEffect } from "./animation.js";

interface WeatherData { // weather api interface -->
    name: string
    main: {
        temp: number
    }
    weather: {
        icon: string
    } []
}

export async function fetchWeather(city: string){
    const cityName: string = city
    const apiKey: string = '5c277bbcaaa9df27a6ced7c978b5f134'
    const apiUrl = `https://api.openweathermap.org/data/2.5/weather?q=${cityName}&units=metric&appid=${apiKey}&lang=hu`;

    try {
        const response = await fetch(apiUrl)
        if (!response.ok) {
            invalidInputValue()
            throw new Error(`HTTP hiba! Status: ${response.status}`)
        }
        const data = await response.json()
        displayWeather(data)
        showEffect()
        
    } catch (error){
        console.error(`Hiba történt az API hívása közben: ${error}`);
    }

}


function displayWeather(weatherData: WeatherData): void{
    const city: string = weatherData.name
    const temperature: number = weatherData.main.temp
    /* const weatherType: string = weatherData.weather[0].main */ // weather type == ?Rain
    const weatherTypeImgId: string = weatherData.weather[0].icon

    // html elemek
    const temp = document.querySelector('.temperature') as HTMLParagraphElement
    const location = document.querySelector('.location') as HTMLParagraphElement

    // eltuntetes + megjelenites
    const weatherNotFound = document.querySelector('.weatherNotFound') as HTMLDivElement
    const weatherFound = document.querySelector('.weatherFound') as HTMLDivElement

    weatherNotFound.classList.add('hidden')
    weatherNotFound.classList.remove('active-fx')
    weatherFound.classList.add('active-fx')
    weatherFound.classList.remove('hidden')

    temp.innerText = `${String(Math.floor(temperature))}°c`

    
    location.innerText = `${city}`

    console.log(weatherData);
    
    weather_foreacast_img_chooser(weatherTypeImgId)
} 



function weather_foreacast_img_chooser(wtId: string): void{
    const weatherIMG = document.querySelector('#weatherType') as HTMLImageElement

    weatherIMG.src = `https://openweathermap.org/img/wn/${wtId}@2x.png`
}   

function invalidInputValue(): void {
    const weatherNotFound = document.querySelector('.weatherNotFound') as HTMLDivElement
    const weatherFound = document.querySelector('.weatherFound') as HTMLDivElement
    showEffect()

    weatherNotFound.classList.add('active-fx')
    weatherNotFound.classList.remove('hidden')

    weatherFound.classList.add('hidden')
    weatherFound.classList.remove('active-fx')
}