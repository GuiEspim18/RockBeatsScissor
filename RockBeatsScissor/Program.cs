// See https://aka.ms/new-console-template for more information
void Welcome() {
    string name = GetName();
    Console.WriteLine($"Olá {name}, vamos jogar pedra papel e tesoura? Mas diferente?");
    int option = ChooseOptions(new string[]{
        "Como Jogar",
        "Jogar Agora",
        "Estatísticas"
    });
    Console.WriteLine(option);
}

int ChooseOptions(string[] validOptions) {
    Dictionary<int, string> options = new();
    int number = 0;
    foreach (string opt in validOptions) {
        options.Add(number++, opt);
        Console.WriteLine($"({number}) {opt}");
    }
    Console.WriteLine("Escolha uma opção:");
    string? option = Console.ReadKey().KeyChar.ToString();
    bool valid = ValidateOpt(option, number);
    while (!valid) {
        Console.WriteLine("\nOpção inválida:");
        option = Console.ReadKey().KeyChar.ToString();
        valid = ValidateOpt(option, number);
    }
    return int.Parse(option);

}

bool ValidateOpt(string option, int max) {
    if (option != null) {
        int numOpt = int.Parse(option);
        if (numOpt > 0 && numOpt < max) {
            return true;
        }
    }
    return false;
}

string GetName() {
    Console.WriteLine("Qual o seu nome?");
    string? name = Console.ReadLine();
    while (name == null) {
        Console.WriteLine("\nSeu nome não pode ser nulo!");
        name = Console.ReadLine();
    }
    return name;
}

Welcome();
