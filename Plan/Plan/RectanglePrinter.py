z = 0
for y in range(2, 16, 2):
	for x in range(0, 96, 2):
		print(("<Rectangle x:Name=\"r{n}\" Grid.Row=\"{a}\" Grid.Column=\"{b}\" Fill=\"Transparent\" Margin=\"0\" Stroke=\"Transparent\ PointerEntered=\"Rectangle_PointerEntered\"/>".format(n=z, a=x, b=y)))
		z = z+1;