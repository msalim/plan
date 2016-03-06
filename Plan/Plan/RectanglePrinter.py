z = 0
for y in range(2, 16, 2):
	for x in range(0, 96, 2):
		#print(("<Rectangle x:Name=\"r{n}\" Grid.Row=\"{a}\" Grid.Column=\"{b}\" Fill=\"Transparent\" Margin=\"0\" Stroke=\"Transparent\" Tapped=\"Rectangle_Tapped\" Tag=\"r{n}\"/>".format(n=z, a=x, b=y)))
		#print(("<TextBlock x:Name=\"t{n}\" Grid.Row=\"{a}\" Grid.Column=\"{b}\" TextWrapping=\"Wrap\" Text=\"\" Grid.RowSpan=\"10\"/>".format(n=z, a=x, b=y)))
		#print("else if(first == {n}) t{n}.Text = eventName;".format(n=z));
		print("if(first <= {n} && {n} <= last) r{n}.Fill = col;".format(n=z));
		z = z+1;