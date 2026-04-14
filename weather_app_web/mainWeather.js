import { fetchWeather } from "./dist/getApi.js";

function start(){
    const searchbar = document.querySelector('#searchbar')
    const search = document.querySelector('#search')

    search.addEventListener('click', () => {
        fetchWeather(searchbar.value)
    })
}


document.addEventListener('DOMContentLoaded', () => {
    start()
})
