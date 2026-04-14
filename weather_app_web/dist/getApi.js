var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
import { showEffect } from "./animation.js";
export function fetchWeather(city) {
    return __awaiter(this, void 0, void 0, function* () {
        const cityName = city;
        const apiKey = '5c277bbcaaa9df27a6ced7c978b5f134';
        const apiUrl = `https://api.openweathermap.org/data/2.5/weather?q=${cityName}&units=metric&appid=${apiKey}&lang=hu`;
        try {
            const response = yield fetch(apiUrl);
            if (!response.ok) {
                invalidInputValue();
                throw new Error(`HTTP hiba! Status: ${response.status}`);
            }
            const data = yield response.json();
            displayWeather(data);
            showEffect();
        }
        catch (error) {
            console.error(`Hiba történt az API hívása közben: ${error}`);
        }
    });
}
function displayWeather(weatherData) {
    const city = weatherData.name;
    const temperature = weatherData.main.temp;
    /* const weatherType: string = weatherData.weather[0].main */ // weather type == ?Rain
    const weatherTypeImgId = weatherData.weather[0].icon;
    // html elemek
    const temp = document.querySelector('.temperature');
    const location = document.querySelector('.location');
    // eltuntetes + megjelenites
    const weatherNotFound = document.querySelector('.weatherNotFound');
    const weatherFound = document.querySelector('.weatherFound');
    weatherNotFound.classList.add('hidden');
    weatherNotFound.classList.remove('active-fx');
    weatherFound.classList.add('active-fx');
    weatherFound.classList.remove('hidden');
    temp.innerText = `${String(Math.floor(temperature))}°c`;
    location.innerText = `${city}`;
    console.log(weatherData);
    weather_foreacast_img_chooser(weatherTypeImgId);
}
function weather_foreacast_img_chooser(wtId) {
    const weatherIMG = document.querySelector('#weatherType');
    weatherIMG.src = `https://openweathermap.org/img/wn/${wtId}@2x.png`;
}
function invalidInputValue() {
    const weatherNotFound = document.querySelector('.weatherNotFound');
    const weatherFound = document.querySelector('.weatherFound');
    showEffect();
    weatherNotFound.classList.add('active-fx');
    weatherNotFound.classList.remove('hidden');
    weatherFound.classList.add('hidden');
    weatherFound.classList.remove('active-fx');
}
