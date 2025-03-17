// See https://aka.ms/new-console-template for more information
Dictionary<string, (int, int, int)> statitics = new();
string[] mainMenu = new string[]{
        "Tutorial",
        "Jogar Agora",
        "Adicionar Usuário",
        "Estatísticas",
        "Sair"
};
string[] gameOptions = new string[]{
    "Pedra",
    "Papel", 
    "Tesoura"
};
string user;

void Welcome()
{
    Console.WriteLine("--Pedra, Papel e Tesoura--");
    int option = ChooseOptions(mainMenu);
    while (option != 5)
    {
        Choice(option);
        option = ChooseOptions(mainMenu);
    }
}

void Play()
{
    if (statitics.Count == 0)
    {
        AddUser();
    }
    Console.WriteLine("--Vamos jogar--");
    ChooseUser();
    Console.WriteLine("Mão 1:");
    int firstHand = ChooseOptions(gameOptions);
    Console.WriteLine("Mão 2:");
    int secondHand = ChooseOptions(gameOptions);
}

void ChooseUser() {
    Console.WriteLine("Com qual usuário você quer jogar ?");
    string[] keys = statitics.Keys.ToArray();
    int option = ChooseOptions(keys);
    user = keys[option - 1];
}

void Tutorial()
{
    Console.WriteLine("--Tutorial--");
    Console.WriteLine("Cada jogador usa as duas mãos, jogando duas vezes (uma com cada mão) e recebendo valores de pedra, papel ou tesoura. Depois, escolhe um dos valores para enfrentar o adversário. O vencedor é decidido pelas regras tradicionais: pedra vence tesoura, tesoura vence papel e papel vence pedra.");
}

void Choice(int option)
{
    switch (option)
    {
        case 1: Tutorial(); break;
        case 2: Play(); break;
        case 3: AddUser(); break;
        case 4: Console.WriteLine(""); break;
        case 5: Console.WriteLine(""); break;
    }
}

void AddUser()
{
    Console.WriteLine("Digite o seu nome:");
    string name = GetName();
    while (statitics.ContainsKey(name))
    {
        Console.WriteLine("Este usuário já foi adicionado!");
        name = GetName();
    }
    statitics.Add(name, (0, 0, 0));
}

int ChooseOptions(string[] validOptions)
{
    Dictionary<int, string> options = new();
    int number = 0;
    foreach (string opt in validOptions)
    {
        options.Add(number++, opt);
        Console.WriteLine($"({number}) {opt}");
    }
    Console.WriteLine("Escolha uma opção:");
    string? option = Console.ReadKey().KeyChar.ToString();
    Console.WriteLine();
    bool valid = ValidateOpt(option, number);
    while (!valid)
    {
        Console.WriteLine("Opção inválida:");
        option = Console.ReadKey().KeyChar.ToString();
        Console.WriteLine();
        valid = ValidateOpt(option, number);
    }
    return int.Parse(option);

}

bool ValidateOpt(string option, int max)
{
    if (option != null)
    {
        int numOpt = int.Parse(option);
        if (numOpt > 0 && numOpt <= max)
        {
            return true;
        }
    }
    return false;
}

string GetName()
{
    Console.WriteLine("Qual o seu nome?");
    string? name = Console.ReadLine();
    Console.WriteLine();
    while (name == null)
    {
        Console.WriteLine("Seu nome não pode ser nulo!");
        name = Console.ReadLine();
        Console.WriteLine();
    }
    return name;
}

Welcome();
