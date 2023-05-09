function CalcularNivelDeRiesgo(niveles, probabilidad, impacto, riesgoInherente) {
    var data = {};
    for (var i = 0; i < niveles.length; i++) {
        var valor = niveles[i].valor;
        var minimo = parseInt(niveles[i].auxiliar1); if (minimo === 0) minimo = -1;
        var maximo = parseInt(niveles[i].auxiliar2);
        var backgroundColor = niveles[i].auxiliar3;
        var color = niveles[i].auxiliar4;
        if (riesgoInherente > minimo && riesgoInherente <= maximo) {
            data.backgroundColor = backgroundColor;
            data.color = color;
            data.nivelRiesgo = valor;
            break;
        }
    }
    return data;
}