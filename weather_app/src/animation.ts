/* 
interface User {
    name: string
    greet: () => void
}
export function userAdd(user: string): User {
    const output = document.querySelector('#output') as HTMLParagraphElement

    const person: User = {
        name: user,
        greet: function() {
            output.innerText = greeting(this.name)
        }
    }

    return person
}
function greeting(user: string): string {
    return `Hello ${user}`
}
 */

// example end  *************


// megjelenesi animacio

export function showEffect(){
    const animation = `
    .weatherForeacast {
        visibility: visible;
        opacity: 1;
        max-height: 500px;
        padding: 30px;
    }`

    styleLayout(animation)
}

function styleLayout(animation: string): void {
    const style = document.createElement('style') as HTMLStyleElement
    style.textContent = animation
    document.head.appendChild(style)
}

